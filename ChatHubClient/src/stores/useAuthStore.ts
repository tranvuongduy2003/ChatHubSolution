import { create } from "zustand";
import { persist, createJSONStorage } from "zustand/middleware";
import { User } from "../interfaces/user.interface";
import { TokenPayload } from "../interfaces/auth.interface";

type State = {
  token: TokenPayload;
  loggedIn: boolean;
  profile: User | null;
};

type Action = {
  setToken: (token: TokenPayload) => void;
  setLoggedIn: (status: boolean) => void;
  setProfile: (profile: User) => void;
  reset: () => void;
};

const initState: State = {
  token: {
    accessToken: "",
    refreshToken: "",
  },
  loggedIn: false,
  profile: null,
};

export const useAuthStore = create(
  persist<State & Action>(
    (set) => ({
      ...initState,
      setToken: (token: TokenPayload) => set(() => ({ token })),
      setLoggedIn: (status: boolean) => set(() => ({ loggedIn: status })),
      setProfile: (updatedProfile: Partial<User>) =>
        set((state) => ({
          profile: { ...state.profile, ...(updatedProfile as User) },
        })),
      reset: () => set({ ...initState }),
    }),
    {
      name: "auth", // unique name
      storage: createJSONStorage(() => localStorage),
    }
  )
);
