import React, { useRef, useState } from 'react';
import { createUserWithEmailAndPassword } from 'firebase/auth';
import { auth } from '../firebase';
import { useNavigate } from 'react-router-dom';

export default function Register() {
  const videoRef = useRef(null);
  const navigate = useNavigate();
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [streamStarted, setStreamStarted] = useState(false);

  const startCamera = async () => {
    try {
      const stream = await navigator.mediaDevices.getUserMedia({ video: true });
      videoRef.current.srcObject = stream;
      setStreamStarted(true);
    } catch (error) {
      console.error('Error accessing camera', error);
    }
  };

  const captureImageBlob = () => {
    const video = videoRef.current;
    const canvas = document.createElement('canvas');
    canvas.width = video.videoWidth;
    canvas.height = video.videoHeight;
    const ctx = canvas.getContext('2d');
    ctx.drawImage(video, 0, 0);

    return new Promise((resolve) => {
      canvas.toBlob((blob) => {
        resolve(blob);
      }, 'image/jpeg');
    });
  };

  const handleRegister = async (e) => {
    e.preventDefault();
    if (!email || !password) {
      alert('Введите email и пароль');
      return;
    }

    try {
      const blob = await captureImageBlob();
      if (!blob) {
        alert('Не удалось захватить фото');
        return;
      }

      // 1. Отправляем фото на backend для RAM хранения
      const formData = new FormData();
      formData.append('photo', new File([blob], 'face.jpg'));
      formData.append('email', email);

      await fetch('http://127.0.0.1:8000/register_face', {
        method: 'POST',
        body: formData,
      });

      // 2. Регистрируем в Firebase
      await createUserWithEmailAndPassword(auth, email, password);

      // 3. После успешной регистрации — переход на логин
      alert('Регистрация успешна! Теперь войдите.');
      navigate('/login');
    } catch (err) {
      console.error('Ошибка при регистрации:', err);
      alert('Ошибка при регистрации');
    }
  };

  return (
    <div className="flex justify-center items-center h-screen bg-gray-100">
      <form onSubmit={handleRegister} className="bg-white p-8 rounded shadow-md w-full max-w-md">
        <h2 className="text-2xl font-bold text-center mb-6">Register</h2>

        <input
          type="email"
          placeholder="Email"
          className="w-full border px-4 py-2 mb-4 rounded"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          required
        />

        <input
          type="password"
          placeholder="Password"
          className="w-full border px-4 py-2 mb-4 rounded"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          required
        />

        <video
          ref={videoRef}
          autoPlay
          muted
          className="w-full h-[200px] rounded mb-4 bg-black"
        />

        <div className="flex justify-center gap-2 mb-4">
          <button
            type="button"
            onClick={startCamera}
            className="bg-gray-300 px-4 py-2 rounded"
          >
            Start Camera
          </button>
        </div>

        <button
          type="submit"
          className="w-full bg-blue-600 text-white py-2 rounded hover:bg-blue-700 transition"
        >
          Register
        </button>
      </form>
    </div>
  );
}
