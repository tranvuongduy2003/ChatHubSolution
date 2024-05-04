import { useAppStore } from "@/stores/useAppStore";
import { useAuthStore } from "@/stores/useAuthStore";

export const logOut = () => {
  const setIsLoading = useAppStore.getState().setIsLoading;
  const reset = useAuthStore.getState().reset;

  setIsLoading(true);
  reset();

  setIsLoading(false);
};

export const getAccessToken = () => {
  const token = useAuthStore.getState().token;
  return token;
};
