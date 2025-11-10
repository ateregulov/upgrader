import React, { useEffect, useState } from 'react';
import api from '../../../api'

const TestHeaders: React.FC = () => {
  const [data, setData] = useState<string | null>(null);

  useEffect(() => {
    const fetchUsers = async () => {
      try {
        const response = await api.getHeaders() as string;
        setData(response);
      } catch (err) {
        console.error('Error fetching headers:', err);
      }
    };

    fetchUsers();
  }, []);

  return (
    <div className="container mx-auto p-4">
      <div><pre>{JSON.stringify(data, null, 2) }</pre></div>
    </div>
  );
};

export default TestHeaders; 