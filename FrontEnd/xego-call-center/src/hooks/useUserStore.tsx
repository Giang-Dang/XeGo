import { create } from "zustand";
import { persist } from "zustand/middleware";
import UserDto from "../models/dto/UserDto";

export interface UserState {
  user: UserDto | null;
  setUser: (user: UserDto | null) => void;
  roles: string[] | null;
  setRoles: (roles: string[] | null) => void;

}
export const useUserStore = create<UserState>()(
  persist(
    (set) => ({
      user: null,
      setUser: (user) => set({ user }),
      roles: null,
      setRoles: (roles) => set({ roles }) 
    }),
    {
      name: "app-storage",
      partialize: (state) => ({
        user: state.user,
        roles: state.roles,
      }),
    }
  )
);