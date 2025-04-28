export const uploadFaceId = async (descriptorArray) => {
  const blob = new Blob([JSON.stringify(descriptorArray)], { type: 'application/json' });
  const file = new File([blob], 'faceId.json', { type: 'application/json' });

  const formData = new FormData();
  formData.append('files', file);

  const response = await fetch('https://uploadthing.com/api/uploadFiles', {
    method: 'POST',
    headers: {
      'Authorization': 'Bearer eyJhcGlLZXkiOiJza19saXZlXzMwOTNhNDNlNTcyMDZjODM0YTI3ZGUyNTExNTVhMjczZmIyMzFkNDlhNTgyMzQwZWRhNmFiZmQ1MTkwYWUxOTMiLCJhcHBJZCI6IjBlYWF2MXpuN3AiLCJyZWdpb25zIjpbInNlYTEiXX0='
    },
    body: formData,
  });

  const data = await response.json();
  return data[0].fileUrl; // ссылка на загруженный faceId.json
};
