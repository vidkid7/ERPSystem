import { createSlice, PayloadAction } from '@reduxjs/toolkit';

interface UIState {
  sidebarCollapsed: boolean;
  currentModule: string;
  breadcrumbs: { title: string; path?: string }[];
}

const initialState: UIState = {
  sidebarCollapsed: false,
  currentModule: 'dashboard',
  breadcrumbs: [],
};

const uiSlice = createSlice({
  name: 'ui',
  initialState,
  reducers: {
    toggleSidebar: (state) => { state.sidebarCollapsed = !state.sidebarCollapsed; },
    setCurrentModule: (state, action: PayloadAction<string>) => { state.currentModule = action.payload; },
    setBreadcrumbs: (state, action: PayloadAction<{ title: string; path?: string }[]>) => {
      state.breadcrumbs = action.payload;
    },
  },
});

export const { toggleSidebar, setCurrentModule, setBreadcrumbs } = uiSlice.actions;
export default uiSlice.reducer;
