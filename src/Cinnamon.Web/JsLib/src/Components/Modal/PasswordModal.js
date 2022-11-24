import axios from "axios";

const passwordModal = {};

passwordModal._initFormLogin = () => {
  $("#form-submit-login").submit(async (e) => {
    e.preventDefault();

    const email = $("#login-email-hidden").val();
    const password = $("#text-login-password").val();

    const errors = [];
    if (!email) errors.push("Please provide your email address");
    if (!password) errors.push("Please provide your password");

    if (errors.length > 0) {
      passwordModal._showError(errors);
      return;
    }

    try {
      const payload = {
        email,
        password,
      };

      passwordModal._hideError();
      const { data } = await axios.post("/api/account/login", payload);
      if (data.success) {
        window.location = "/explore";
      } else {
        const errors = [data.message];
        passwordModal._showError(errors);
      }
    } catch {
      const errors = ["An error occured please try again later"];
      passwordModal._showError(errors);
    }
  });
};

passwordModal._showError = (errors) => {
  let errorHTML = "";
  $("#error-list-login").empty();
  errors.forEach((item) => {
    errorHTML += `<li style="font-family:'Nunito';font-size:14px;">${item}</li>`;
  });
  $("#error-list-login").append(errorHTML);
  $(".error-message-container-submit-login").show();
};

passwordModal._hideError = () => {
  $("#error-list-login").empty();
  $(".error-message-container-submit-login").hide();
};

passwordModal.init = () => {
  passwordModal._hideError();
  passwordModal._initFormLogin();
};

export default passwordModal;
