import React from 'react';
import { ConfigProvider } from 'antd';
import { Provider } from 'react-redux';
import { store } from './store';
import AppRouter from './hooks/AppRouter';

const App: React.FC = () => {
  return (
    <Provider store={store}>
      <ConfigProvider
        theme={{
          token: {
            colorPrimary: '#1677ff',
            borderRadius: 6,
          },
        }}
      >
        <AppRouter />
      </ConfigProvider>
    </Provider>
  );
};

export default App;
