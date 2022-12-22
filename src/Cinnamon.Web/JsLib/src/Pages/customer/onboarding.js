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
        debugger;
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

onboarding._initTouchSwipe = () => {
  $(document).ready(function () {
    $(".carousel").carousel({
      interval: false,
      pause: true,
    });

    $(".carousel .carousel-inner").swipe({
      swipeLeft: function (event, direction, distance, duration, fingerCount) {
        this.parent().carousel("next");
      },
      swipeRight: function () {
        this.parent().carousel("prev");
      },
      threshold: 0,
      tap: function (event, target) {
        window.location = $(this).find(".carousel-item.active a").attr("href");
      },
      excludedElements: "label, button, input, select, textarea, .noSwipe",
    });

    $(".carousel .carousel-inner").on("dragstart", "a", function () {
      return false;
    });
  });
};

onboarding.init = () => {
  console.log("onboarding page initiated");
  onboarding._initEmailSignup();
  onboarding._initFormRegister();
  onboarding._initTouchSwipe();
};

export default onboarding;
