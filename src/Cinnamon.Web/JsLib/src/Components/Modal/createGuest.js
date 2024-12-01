import axios from "axios";

export default async function createGuest(obj) {
  try {
    const { data } = await axios.post("/api/account/CreateGuestCustomer", obj);
    return { success: data.success, message: data.message };
  } catch (error) {
    return {
      success: false,
      message: "An errorr occured. Please try again later",
    };
  }
}
