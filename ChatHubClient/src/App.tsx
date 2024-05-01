import { Route, Routes } from "react-router-dom";
import AuthProtectedRoute from "./layout/AuthProtectedRoute";
import { LoginPage } from "./pages/auth/LoginPage";
import { SignUpPage } from "./pages/auth/SignUpPage";
import { HomePage } from "./pages/home/HomePage";
import HomeLayout from "./layout/HomeLayout";

function App() {
  return (
    <Routes>
      <Route>
        {/* Auth Route */}
        <Route element={<AuthProtectedRoute />}>
          <Route path="/auth/login" element={<LoginPage />} />
          <Route path="/auth/sign-up" element={<SignUpPage />} />
        </Route>

        <Route element={<HomeLayout />}>
          {/* Products */}
          <Route path="/" element={<HomePage />} />
        </Route>
      </Route>
    </Routes>
  );
}

export default App;
