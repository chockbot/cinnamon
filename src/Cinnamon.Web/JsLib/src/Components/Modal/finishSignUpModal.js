import axios from "axios";
import { track } from "../../mixpanel_lib";

const finishSignupModal = {};

finishSignupModal.init = async (obj) => {
debugger;
  try {
    const { data } = await axios.post("/api/account/registerautologin", obj);
    if (data.success) {
      track("User Signup");
    }
    return { success: data.success, message: data.message };
  } catch (ex) {
    return {
      success: false,
      message: "An errorr occured. Please try again later",
    };
  }
};

export default finishSignupModal;
