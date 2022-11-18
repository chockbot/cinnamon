import axios from "axios";

const onboarding = {};

onboarding._initEmailSignup = () => {
  $("#email-submit").submit(async (e) => {
    e.preventDefault();

    const email = $("#email-signup").val();
    if (email) {
      $("#email-spinner-loading").removeClass("d-none");

      try {
        const { data } = await axios.post("/api/account/Register", { email });
        if (data.success) {
          $("#hidden-email").val(email);
          switch (data.code) {
            case "NOTVERIFIED":
              const notVerifiedModal = new bootstrap.Modal(
                document.getElementById("verificationModal")
              );
              notVerifiedModal.show();
              break;
            case "NOTREGISTERED":
              const noRegistered = new bootstrap.Modal(
                document.getElementById("finishModal")
              );
              $("#email-signup-register").val($("#email-signup").val());
              noRegistered.show();
              break;
            case "VERIFIED":
              const passwordModal = new bootstrap.Modal(
                document.getElementById("login-password-modal")
              );
              passwordModal.show();
              break;
            case "EMAILREGISTERED":
              const newRegistered = new bootstrap.Modal(
                document.getElementById("verificationModal")
              );
              newRegistered.show();
              break;
          }
        }
      } catch {
      } finally {
        $("#email-spinner-loading").addClass("d-none");
      }
    }
  });
};

onboarding._initFormRegister = () => {
  $("#form-register-information").submit(async function (e) {
    e.preventDefault();

    const firstname = $("#firstname-signup-register").val();
    const lastname = $("#lastname-signup-register").val();
    const birthdate = $("#birtdate-signup-register").val();
    const email = $("#email-signup-register").val();
    const password = $("#password-signup-register").val();
    const confirmPassword = $("#confirm-signup-register").val();

    const errors = [];
    if (!firstname) errors.push("Please provide your First Name");
    if (!lastname) errors.push("Please provide your Last Name");
    if (!birthdate) errors.push("Please provide your Birthdate");
    if (!email) errors.push("Please provide your email address");
    if (!password) errors.push("Please provide your password");
    // prettier-ignore
    if(confirmPassword !== password) errors.push("Password mismatch");

    let errorHTML = "";
    errors.forEach((item) => {
      errorHTML += `<li style="font-family:'Nunito';font-size:14px;">${item}</li>`;
    });
    $("#error-list-register").append(errorHTML);

    if (errors.length > 0) return;

    $("#spinner-loading-register").removeClass("d-none");
    try {
      const payload = {
        firstname,
        lastname,
        email,
        birthdate,
        password,
      };

      const { data } = await axios.post(
        "/api/account/registerautologin",
        payload
      );
      if (data.success) {
        window.location = "/explore";
      }
    } catch {
    } finally {
      $("#spinner-loading-register").addClass("d-none");
    }
  });
};

onboarding.init = () => {
  console.log("onboarding page initiated");
  onboarding._initEmailSignup();
  onboarding._initFormRegister();
};

export default onboarding;
