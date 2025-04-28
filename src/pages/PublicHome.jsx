import React from "react";

export default function PublicHome() {
  return (
    <div className="min-h-screen bg-white text-center flex flex-col justify-center items-center px-4">
      <h1 className="text-4xl font-bold text-sky-600 mb-4">
        Welcome to Astana IT University Platform
      </h1>
      <p className="text-gray-600 text-lg mb-8 max-w-xl">
        This educational platform allows students to check their grades,
        confirm attendance using Face ID, and stay connected with teachers in real-time.
      </p>
      <div className="flex gap-4">
        <a href="/login" className="bg-sky-600 text-white px-6 py-3 rounded-lg text-lg hover:bg-sky-700 transition">
          Login
        </a>
        <a href="/register" className="bg-gray-200 px-6 py-3 rounded-lg text-lg hover:bg-gray-300 transition">
          Register
        </a>
      </div>
    </div>
  );
}
