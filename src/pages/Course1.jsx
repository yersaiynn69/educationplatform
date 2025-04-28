import React, { useEffect, useRef, useState } from 'react';
import * as faceapi from 'face-api.js';

export default function Course1() {
  const videoRef = useRef(null);
  const [email, setEmail] = useState('');
  const [message, setMessage] = useState('');
  const [time, setTime] = useState(new Date().toLocaleTimeString());

  useEffect(() => {
    const timer = setInterval(() => {
      setTime(new Date().toLocaleTimeString());
    }, 1000);

    return () => clearInterval(timer);
  }, []);

  useEffect(() => {
    const loadModels = async () => {
      const MODEL_URL = '/models';
      await Promise.all([
        faceapi.nets.tinyFaceDetector.loadFromUri(MODEL_URL),
        faceapi.nets.faceRecognitionNet.loadFromUri(MODEL_URL),
        faceapi.nets.faceLandmark68Net.loadFromUri(MODEL_URL)
      ]);
    };
    loadModels();
  }, []);

  const startCamera = async () => {
    if (navigator.mediaDevices.getUserMedia) {
      const stream = await navigator.mediaDevices.getUserMedia({ video: true });
      videoRef.current.srcObject = stream;
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

  const handleAttendance = async () => {
    if (!email) {
      alert('Введите email');
      return;
    }

    const blob = await captureImageBlob();
    if (!blob) {
      alert('Не удалось захватить изображение.');
      return;
    }

    const formData = new FormData();
    formData.append('email', email);
    formData.append('photo', new File([blob], 'face.jpg'));

    try {
      const res = await fetch('http://localhost:8000/verify_face', {
        method: 'POST',
        body: formData
      });

      const data = await res.json();
      if (data.distance !== undefined) {
        if (data.distance < 10.0) {
          setMessage(`✅ Присутствие подтверждено! (Distance: ${data.distance.toFixed(2)})`);
        } else {
          setMessage(`❌ Лицо не совпадает (Distance: ${data.distance.toFixed(2)})`);
        }
      } else {
        setMessage(`❌ ${data.message}`);
      }
    } catch (err) {
      console.error(err);
      setMessage('Ошибка при подтверждении.');
    }
  };

  const currentHour = new Date().getHours();
  const isAttendanceTime = currentHour >= 12 && currentHour < 13;

  return (
    <div className="flex">
      <div className="w-1/5 bg-gray-800 text-white min-h-screen p-4">
        <h2 className="text-2xl font-bold mb-6">Sidebar</h2>
        <ul>
          <li className="mb-4"><a href="/home">Home</a></li>
          <li className="mb-4"><a href="/course1">Course 1</a></li>
          <li className="mb-4"><a href="/course2">Course 2</a></li>
          <li className="mb-4"><a href="/course3">Course 3</a></li>
        </ul>
      </div>

      <div className="w-4/5 p-8">
        <h1 className="text-3xl font-bold mb-6">Course 1 - Attendance</h1>

        <input
          type="email"
          placeholder="Enter your email"
          className="w-full border px-4 py-2 mb-4 rounded"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
        />

        <video
          ref={videoRef}
          autoPlay
          muted
          className="w-full h-[250px] rounded mb-4 bg-black"
        />

        <div className="flex gap-4 mb-6">
          <button onClick={startCamera} className="bg-gray-500 text-white px-4 py-2 rounded">
            Start Camera
          </button>
          {isAttendanceTime && (
            <button onClick={handleAttendance} className="bg-blue-600 text-white px-4 py-2 rounded">
              Confirm Attendance
            </button>
          )}
        </div>

        {message && (
          <p className={`text-lg ${message.includes('✅') ? 'text-green-600' : 'text-red-600'} mb-6`}>
            {message}
          </p>
        )}

        <h2 className="text-2xl font-bold mb-4">Grades</h2>
        <div className="grid grid-cols-3 gap-4">
          <div className="border p-4 rounded">
            <h3 className="font-semibold">Midterm</h3>
            <p>-</p>
          </div>
          <div className="border p-4 rounded">
            <h3 className="font-semibold">Endterm</h3>
            <p>-</p>
          </div>
          <div className="border p-4 rounded">
            <h3 className="font-semibold">Final Exam</h3>
            <p>-</p>
          </div>
        </div>
      </div>
    </div>
  );
}
