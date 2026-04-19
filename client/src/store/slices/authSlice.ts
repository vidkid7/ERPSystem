import { createSlice, createAsyncThunk, PayloadAction } from '@reduxjs/toolkit';
import api from '../../services/api';
import type { LoginRequest, LoginResponse } from '../../types';

interface AuthState {
  user: LoginResponse | null;
  isAuthenticated: boolean;
  loading: boolean;
  error: string | null;
}

const initialState: AuthState = {
  user: JSON.parse(localStorage.getItem('erp_user') || 'null'),
  isAuthenticated: !!localStorage.getItem('erp_token'),
  loading: false,
  error: null,
};

export const login = createAsyncThunk('auth/login', async (credentials: LoginRequest, { rejectWithValue }) => {
  try {
    const { data } = await api.post('/auth/login', credentials);
    if (data.isSuccess) {
      localStorage.setItem('erp_token', data.data.token);
      localStorage.setItem('erp_refresh_token', data.data.refreshToken);
      localStorage.setItem('erp_user', JSON.stringify(data.data));
      return data.data as LoginResponse;
    }
    return rejectWithValue(data.responseMSG);
  } catch (err: any) {
    return rejectWithValue(err.response?.data?.responseMSG || 'Login failed');
  }
});

const authSlice = createSlice({
  name: 'auth',
  initialState,
  reducers: {
    logout: (state) => {
      state.user = null;
      state.isAuthenticated = false;
      localStorage.removeItem('erp_token');
      localStorage.removeItem('erp_refresh_token');
      localStorage.removeItem('erp_user');
    },
    clearError: (state) => { state.error = null; },
  },
  extraReducers: (builder) => {
    builder
      .addCase(login.pending, (state) => { state.loading = true; state.error = null; })
      .addCase(login.fulfilled, (state, action: PayloadAction<LoginResponse>) => {
        state.loading = false;
        state.isAuthenticated = true;
        state.user = action.payload;
      })
      .addCase(login.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
      });
  },
});

export const { logout, clearError } = authSlice.actions;
export default authSlice.reducer;
