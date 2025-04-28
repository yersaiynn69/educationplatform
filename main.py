from fastapi import FastAPI, UploadFile, Form
from fastapi.middleware.cors import CORSMiddleware
import uvicorn
import shutil
import os
import numpy as np
from PIL import Image
from datetime import datetime
from insightface.app import FaceAnalysis

app = FastAPI()

# Настройка CORS
app.add_middleware(
    CORSMiddleware,
    allow_origins=["*"],
    allow_methods=["*"],
    allow_headers=["*"]
)

# Инициализация FaceAnalysis
face_app = FaceAnalysis(name="buffalo_l", providers=["CPUExecutionProvider"])
face_app.prepare(ctx_id=0)

# RAM-хранилище для лиц
registered_faces = {}

@app.post("/register_face")
async def register_face(email: str = Form(...), photo: UploadFile = Form(...)):
    try:
        temp_path = f"temp_{photo.filename}"
        with open(temp_path, "wb") as buffer:
            shutil.copyfileobj(photo.file, buffer)

        img = np.array(Image.open(temp_path).convert("RGB"))
        faces = face_app.get(img)

        if not faces:
            os.remove(temp_path)
            return {"message": "Лицо не найдено"}

        descriptor = faces[0].embedding
        registered_faces[email] = descriptor
        os.remove(temp_path)

        return {"message": "Лицо успешно сохранено"}
    except Exception as e:
        return {"message": f"Ошибка: {str(e)}"}

@app.post("/verify_face")
async def verify_face(email: str = Form(...), photo: UploadFile = Form(...)):
    try:
        if email not in registered_faces:
            return {"message": "Пользователь не найден"}

        temp_path = f"temp_verify_{photo.filename}"
        with open(temp_path, "wb") as buffer:
            shutil.copyfileobj(photo.file, buffer)

        img = np.array(Image.open(temp_path).convert("RGB"))
        faces = face_app.get(img)

        if not faces:
            os.remove(temp_path)
            return {"message": "Лицо не обнаружено"}

        descriptor = faces[0].embedding
        saved_descriptor = registered_faces[email]

        distance = np.linalg.norm(saved_descriptor - descriptor)
        os.remove(temp_path)

        if distance < 10.0:
            return {"message": "Аттенданс подтвержден", "distance": float(distance)}
        else:
            return {"message": "Лицо не совпадает", "distance": float(distance)}
    except Exception as e:
        return {"message": f"Ошибка: {str(e)}"}

if __name__ == "__main__":
    uvicorn.run("main:app", host="127.0.0.1", port=8000, reload=True)

