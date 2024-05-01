import { refresh } from "../apis/auth.api";
import { useAppStore } from "../stores/useAppStore";
import { useAuthStore } from "../stores/useAuthStore";

export const logOut = () => {
  const setIsLoading = useAppStore.getState().setIsLoading;
  const reset = useAuthStore.getState().reset;

  setIsLoading(true);
  reset();

  setIsLoading(false);
};

export const getAccessToken = () => {
  const token = useAuthStore.getState().token.accessToken;
  return token;
};

export const getRefreshToken = () => {
  const token = useAuthStore.getState().token.refreshToken;
  return token;
};

export const handleRefreshToken = async () => {
  const refreshToken = getRefreshToken();
  const { data } = await refresh(refreshToken);
  const setToken = useAuthStore.getState().setToken;
  setToken({ refreshToken, accessToken: data as string });
  return data;
};
