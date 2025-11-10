import React, { useEffect, useState } from 'react';
import api from '../../../api'
import TestUser from './TestUser'

const TestApi: React.FC = () => {
  const [users, setUsers] = useState<TestUser[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  const fetchUsers = async () => {
    try {
      const response = await api.getTestUsers() as TestUser[];
      setUsers(response);
    } catch (err) {
      setError('Ошибка при загрузке данных');
      console.error('Error fetching users:', err);
    } finally {
      setLoading(false);
    }
  };

  const addUser = async () => {
    try {
      const user: TestUser = {
        id: 0,
        name: 'Test3',
        age: 30,
      };
      const response = await api.createTestUser(user)
    } catch (err) {
      setError('Ошибка при добавлении пользователя');
      console.error('Error adding user:', err);
    }
  };

  useEffect(() => {
    async function wrapperAsync() {
      await addUser();
      await fetchUsers();
    }

    wrapperAsync()
  }, []);

  if (loading) {
    return <div>Загрузка...</div>;
  }

  if (error) {
    return <div>Ошибка: {error}</div>;
  }

  return (
    <div className="container mx-auto p-4">
      <h1 className="text-2xl font-bold mb-4">Тестовая страница</h1>
      <div className="grid gap-4">
        {users.map((user) => (
          <div key={user.id} className="border p-4 rounded-lg shadow">
            <h2 className="text-xl font-semibold">{user.name}</h2>
            <p className="text-gray-600">{user.age}</p>
          </div>
        ))}
      </div>
    </div>
  );
};

export default TestApi; 