import axios from "axios";
import { track } from "../../mixpanel_lib";

const finishSignupModal = {};

finishSignupModal._initFinishForm = () => {
  $("#submitFinishSignup").submit(async (e) => {
    e.preventDefault();

    const firstname = $("#firstname-signup-finish").val();
    const lastname = $("#lastname-signup-finish").val();
    const birthdate = $("#birtdate-signup-finish").val();
    const email = $("#email-signup-finish").val();
    const password = $("#password-signup-finish").val();
    const confirmPassword = $("#confirm-signup-finish").val();

    const errors = [];
    if (!firstname) errors.push("Please provide your First Name");
    if (!lastname) errors.push("Please provide your Last Name");
    if (!birthdate) errors.push("Please provide your Birthdate");
    if (!email) errors.push("Please provide your email address");
    if (!password) errors.push("Please provide your password");
    // prettier-ignore
    if (confirmPassword !== password) errors.push("Password mismatch");

    if (errors.length > 0) {
      finishSignupModal._showErrors(errors);
      return;
    }

    try {
      const payload = {
        firstname,
        lastname,
        email,
        birthdate,
        password,
      };

      finishSignupModal._hideErrors();
      const { data } = await axios.post(
        "/api/account/registerautologin",
        payload
      );

      if (data.success) {
        track("User Signup");
        window.location = "/explore";
      } else {
        const errors = ["An error occured please try again later"];
        finishSignupModal._showErrors(errors);
      }
    } catch (ex) {
      console.log(ex);
      const errors = ["An error occured please try again later"];
      finishSignupModal._showErrors(errors);
    } finally {
    }
  });
};

finishSignupModal._showErrors = (errors) => {
  let errorHTML = "";
  $("#error-list-finish").empty();
  errors.forEach((item) => {
    errorHTML += `<li style="font-family:'Nunito';font-size:14px;">${item}</li>`;
  });
  $("#error-list-finish").append(errorHTML);
  $(".error-message-container-finish").show();
};

finishSignupModal._hideErrors = () => {
  $("#error-list-finish").empty();
  $(".error-message-container-finish").hide();
};

finishSignupModal.init = () => {
  finishSignupModal._hideErrors();
  finishSignupModal._initFinishForm();
};

export default finishSignupModal;
