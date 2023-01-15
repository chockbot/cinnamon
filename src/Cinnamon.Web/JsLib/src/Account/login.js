import axios from "axios";

export default async (email, password) => {
  try {
    const payload = { email, password };
    const { data } = await axios.post("/api/account/login", payload);
    return data;
  } catch {
    return {
      success: false,
      message: "An error occured. Please try again later",
    };
  }
};
