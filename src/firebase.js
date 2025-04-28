import { initializeApp } from "firebase/app";
import { getAuth } from "firebase/auth";
import { getFirestore } from "firebase/firestore";

// Firebase configuration
const firebaseConfig = {
  apiKey: "AIzaSyC82rWDZaHnCCDdkHw9WFgWrCivQJzaT1Y",
  authDomain: "saltanat-ed187.firebaseapp.com",
  projectId: "saltanat-ed187",
  storageBucket: "saltanat-ed187.firebasestorage.app",
  messagingSenderId: "710065053910",
  appId: "1:710065053910:web:8f92777233e51505f2a977"
};

// Initialize Firebase
const app = initializeApp(firebaseConfig);

// Export auth and firestore
export const auth = getAuth(app);
export const db = getFirestore(app);
