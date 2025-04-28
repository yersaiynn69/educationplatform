// src/pages/Home.jsx
import React from 'react';
import { auth } from '../firebase';
import { useNavigate } from 'react-router-dom';

export default function Home() {
  const navigate = useNavigate();
  const user = auth.currentUser;

  const handleSignOut = async () => {
    await auth.signOut();
    navigate('/login');
  };

  return (
    <div className="flex h-screen bg-gray-100">
      {/* Sidebar */}
      <div className="w-64 bg-white shadow-md p-4 flex flex-col">
        <h2 className="text-2xl font-bold mb-6 text-center">Menu</h2>
        <nav className="flex flex-col gap-4">
          <button
            className="text-left px-4 py-2 rounded hover:bg-gray-200 transition"
            onClick={() => navigate('/home')}
          >
            Home
          </button>
          <button
            className="text-left px-4 py-2 rounded hover:bg-gray-200 transition"
            onClick={() => navigate('/dashboard')}
          >
            Dashboard
          </button>
          <button
            className="text-left px-4 py-2 rounded hover:bg-gray-200 transition"
            onClick={() => navigate('/course1')}
          >
            Course 1 - Teacher 1
          </button>
          <button
            className="text-left px-4 py-2 rounded hover:bg-gray-200 transition"
            onClick={() => navigate('/course2')}
          >
            Course 2 - Teacher 2
          </button>
          <button
            className="text-left px-4 py-2 rounded hover:bg-gray-200 transition"
            onClick={() => navigate('/course3')}
          >
            Course 3 - Teacher 3
          </button>
        </nav>
      </div>

      {/* Main Content */}
      <div className="flex-1 flex flex-col">
        {/* Topbar */}
        <div className="flex justify-between items-center bg-white shadow-md p-4">
          <h1 className="text-xl font-bold">Education Platform</h1>
          <div className="flex items-center gap-4">
            <span className="font-medium">{user?.email}</span>
            <button
              onClick={handleSignOut}
              className="bg-red-500 text-white px-4 py-2 rounded hover:bg-red-600 transition"
            >
              Sign Out
            </button>
          </div>
        </div>

        {/* Content */}
        <div className="flex-1 p-6">
          <h2 className="text-2xl font-bold mb-4">Welcome to the Education Platform!</h2>
          <p className="text-gray-700">Please choose a course or view your dashboard from the menu.</p>
        </div>
      </div>
    </div>
  );
}
