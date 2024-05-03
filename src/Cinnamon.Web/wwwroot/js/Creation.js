const NOTHING_PROVIDED = "--NOTHING PROVIDED--";
const formData = new FormData();
const formDataUpdate = new FormData();
var timeout;
var isToggled = false;
var popEventListenerAdded = false;
const imageData = ["#coverPhotoData", "#firstPhotoData", "#secondPhotoData"];
var hasUploadedFile = false;

export function confirmation() {
  $(document).ready(function () {
    if (!popEventListenerAdded) {
      if (window.history && window.history.pushState) {
        window.history.pushState("forward", null, window.location.href);

        $(window).on("popstate", function (event) {
          if (confirm("Are you sure you want to leave this page?")) {
            if (!isToggled) {
              isToggled = true;

              $("#loader").addClass("spinner-border");

              createUpdateActivity();

              $("#regForm *").attr("readonly", "readonly");

              setTimeout(redirectToDashboard, 2000);
            }
          } else {
            console.log("stayed on page");
            history.pushState(null, null, window.location.pathname);
            event.preventDefault();
          }
        });

        $("#app-logo-creation").on("click", function () {
          if (confirm("Are you sure you want to leave this page?")) {
            if (!isToggled) {
              isToggled = true;

              $("#loader").addClass("spinner-border");

              createUpdateActivity();

              $("#regForm *").attr("readonly", "readonly");

              setTimeout(redirectToDashboard, 2000);
            }
          } else {
            console.log("stayed on page");
            history.pushState(null, null, window.location.pathname);
            event.preventDefault();
          }
        });
      }

      popEventListenerAdded = true;
    }

    $("#regForm").on("mousedown", stopNavigate);
  });

  function stopNavigate() {
    $(window).off("beforeunload");
  }
}

export function OffBeforeUnload() {
  $(window).off("beforeunload");
}

export function onScrollUp() {
  window.scrollTo({ top: 0, behavior: "smooth" });
}

export function scrollToRequiredField(container) {
  setTimeout(function () {
    var fieldId = $(`${container} .invalid`).first().attr("id");
    var field = document.getElementById(fieldId);
    var top = field.offsetTop - 420;
    window.scrollTo(0, top);
  }, 200);
}
export function scrollToPricingandSchedule() {
    $('html, body').animate({
        scrollTop: $(".pricing-schedule").offset().top
    }, 200);
}

function replaceEmptyvalue(value, replacement) {
  return value ? value : replacement;
}

function redirectToDashboard() {
  $("#loader").removeClass("spinner-border");

  window.location.href = window.location.origin + "/maker/dashboard";
}

function redirectToExplore() {
  window.location.href = window.location.origin + "/explore";
}

function createUpdateActivity() {
  var activityId = $("#activity-id").val();

  if (activityId && activityId != "0") {
    let counter = 0;
    let scheduleCounter = 0;
    var file = null;
    $(".img-banner").each(function (e) {
      const name = $(this).attr("data-name");
      const hasChanged = $(this).attr("data-changed");
      if (hasChanged !== false && hasChanged !== "false") {
        if (name) {
          file = dataUrlToFile($(imageData[counter]).val(), name);

          if (file) {
            formDataUpdate.append(`Image${counter + 1}`, file);
            counter++;
          }
        }
      } else {
        counter++;
      }
    });

    formDataUpdate.append("ActivityId", activityId);
    formDataUpdate.append(
      "ExperienceTypeId",
      $('input[name="experience-type"]:checked').val()
    );
    formDataUpdate.append(
      "ExperienceCategoryId",
      $("#create-activity-category").val()
    );
    formDataUpdate.append(
      "SubCategoryId",
      $("#create-activity-sub-category").val()
    );
    formDataUpdate.append(
      "Title",
      replaceEmptyvalue($("#create-activity-title").val(), NOTHING_PROVIDED)
    );
    formDataUpdate.append(
      "Description",
      replaceEmptyvalue(
        $("#create-activity-description").val(),
        NOTHING_PROVIDED
      )
    );
    formDataUpdate.append("Price", replaceEmptyvalue(null, NOTHING_PROVIDED));
    formDataUpdate.append(
      "ScheduleIndicator",
      replaceEmptyvalue(null, NOTHING_PROVIDED)
    );
    formDataUpdate.append("Remarks", replaceEmptyvalue(null, NOTHING_PROVIDED));
    formDataUpdate.append(
      "IsPublished",
      $("#activity-is-published").val() == "true" ? true : false
    );
    formDataUpdate.append(
      "Address1",
      replaceEmptyvalue($("#house-number").val(), NOTHING_PROVIDED)
    );
    formDataUpdate.append(
      "Address2",
      replaceEmptyvalue(null, NOTHING_PROVIDED)
    );
    formDataUpdate.append(
      "District",
      replaceEmptyvalue(null, NOTHING_PROVIDED)
    );
    formDataUpdate.append("City", replaceEmptyvalue($("#city").val(), ""));
    formDataUpdate.append(
      "Subdivision",
      replaceEmptyvalue(null, NOTHING_PROVIDED)
    );
    formDataUpdate.append("Region", replaceEmptyvalue($("#region").val(), ""));
    formDataUpdate.append(
      "Barangay",
      replaceEmptyvalue($("#barangay").val(), "")
    );
    formDataUpdate.append(
      "PostalCode",
      replaceEmptyvalue($("#postal-code").val(), "")
    );
    formDataUpdate.append(
      "PinnedLocation",
      replaceEmptyvalue($(".pinned-location").val(), NOTHING_PROVIDED)
    );
    formDataUpdate.append(
      "SpecificsYouWillProvide",
      replaceEmptyvalue(null, NOTHING_PROVIDED)
    );
    formDataUpdate.append(
      "CustomerBringWithThem",
      replaceEmptyvalue($(".ql-editor").html(), NOTHING_PROVIDED)
    );
    formDataUpdate.append(
      "AdditionalRequirements",
      replaceEmptyvalue(null, NOTHING_PROVIDED)
    );
    formDataUpdate.append("ActivityLevel", "Beginner");
    formDataUpdate.append("SkillLevel", "No experience");
    formDataUpdate.append("MinimumAge", 0);
    formDataUpdate.append("CanAdultsJoin", false);
    formDataUpdate.append("IsSetSession", false);
    formDataUpdate.append(
      "SessionName",
      replaceEmptyvalue(null, NOTHING_PROVIDED)
    );
    formDataUpdate.append("SearchTags", [NOTHING_PROVIDED]);
    formDataUpdate.append("Status", parseInt($("#activity-status").val()));

    $(
      "#activitySchedules .accordion-item .accordion-collapse .accordion-body"
    ).each(function () {
      let hasExpiration = 0;

      if ($(this).find("[id^=at-start-]").prop("checked")) hasExpiration = 1;
      else if ($(this).find("[id^=first-attendance-]").prop("checked"))
        hasExpiration = 2;

      formDataUpdate.append(
        "ActivitySchedules[" + scheduleCounter + "].Id",
        $(this).find("#schedule-id").val()
      );
      formDataUpdate.append(
        "ActivitySchedules[" + scheduleCounter + "].Name",
        replaceEmptyvalue(
          $(this).find("#schedule-name").val(),
          NOTHING_PROVIDED
        )
      );
      formDataUpdate.append(
        "ActivitySchedules[" + scheduleCounter + "].DateTime",
        replaceEmptyvalue(
          $(this).find("#schedule-time").val(),
          NOTHING_PROVIDED
        )
      );
      formDataUpdate.append(
        "ActivitySchedules[" + scheduleCounter + "].Price",
        $(this).find("#price-input").val()
      );
      formDataUpdate.append(
        "ActivitySchedules[" + scheduleCounter + "].UnitPrice",
        replaceEmptyvalue("PHP", NOTHING_PROVIDED)
      );
      formDataUpdate.append(
        "ActivitySchedules[" + scheduleCounter + "].PerUnit1",
        1
      );
      formDataUpdate.append(
        "ActivitySchedules[" + scheduleCounter + "].PriceUnit1",
        replaceEmptyvalue("Head", NOTHING_PROVIDED)
      );
      formDataUpdate.append(
        "ActivitySchedules[" + scheduleCounter + "].PerUnit2",
        $(this).find("#unit2-input").val()
      );
      formDataUpdate.append(
        "ActivitySchedules[" + scheduleCounter + "].PriceUnit2",
        replaceEmptyvalue("Session", NOTHING_PROVIDED)
      );
      formDataUpdate.append(
        "ActivitySchedules[" + scheduleCounter + "].Order",
        0
      );
      formDataUpdate.append(
        "ActivitySchedules[" + scheduleCounter + "].IsActiveSchedule",
        $(this).find("#flexSwitchCheckDefault").prop("checked")
      );
      formDataUpdate.append(
        "ActivitySchedules[" + scheduleCounter + "].HasExpiration",
        hasExpiration
      );
      formDataUpdate.append(
        "ActivitySchedules[" + scheduleCounter + "].IsSetSession",
        $(this).find(".set-session").prop("checked")
      );
      formDataUpdate.append(
        "ActivitySchedules[" + scheduleCounter + "].SessionName",
        replaceEmptyvalue($(this).find(".session-period").val(), "")
      );
      formDataUpdate.append(
        "ActivitySchedules[" + scheduleCounter + "].StartDate",
        replaceEmptyvalue(
          $(this).find('input[data-class="start-date"]').val(),
          ""
        )
      );

      scheduleCounter++;
    });

    $(".pricing-schedule").each(function () {
      let hasExpiration = 0;

      if ($(this).find("[id^=at-start-]").prop("checked")) hasExpiration = 1;
      else if ($(this).find("[id^=first-attendance-]").prop("checked"))
        hasExpiration = 2;

      formDataUpdate.append(
        "ActivitySchedules[" + scheduleCounter + "].Id",
        $(this).find("#schedule-id").val()
      );
      formDataUpdate.append(
        "ActivitySchedules[" + scheduleCounter + "].Name",
        replaceEmptyvalue(
          $(this).find("#schedule-name").val(),
          NOTHING_PROVIDED
        )
      );
      formDataUpdate.append(
        "ActivitySchedules[" + scheduleCounter + "].DateTime",
        replaceEmptyvalue(
          $(this).find("#schedule-time").val(),
          NOTHING_PROVIDED
        )
      );
      formDataUpdate.append(
        "ActivitySchedules[" + scheduleCounter + "].Price",
        $(this).find("#price-input").val()
      );
      formDataUpdate.append(
        "ActivitySchedules[" + scheduleCounter + "].UnitPrice",
        replaceEmptyvalue("PHP", NOTHING_PROVIDED)
      );
      formDataUpdate.append(
        "ActivitySchedules[" + scheduleCounter + "].PerUnit1",
        1
      );
      formDataUpdate.append(
        "ActivitySchedules[" + scheduleCounter + "].PriceUnit1",
        replaceEmptyvalue("Head", NOTHING_PROVIDED)
      );
      formDataUpdate.append(
        "ActivitySchedules[" + scheduleCounter + "].PerUnit2",
        $(this).find("#unit2-input").val()
      );
      formDataUpdate.append(
        "ActivitySchedules[" + scheduleCounter + "].PriceUnit2",
        replaceEmptyvalue("Session", NOTHING_PROVIDED)
      );
      formDataUpdate.append(
        "ActivitySchedules[" + scheduleCounter + "].Order",
        0
      );
      formDataUpdate.append(
        "ActivitySchedules[" + scheduleCounter + "].IsActiveSchedule",
        $(this).find("#flexSwitchCheckDefault").prop("checked")
      );
      formDataUpdate.append(
        "ActivitySchedules[" + scheduleCounter + "].HasExpiration",
        hasExpiration
      );
      formDataUpdate.append(
        "ActivitySchedules[" + scheduleCounter + "].IsSetSession",
        $(this).find(".set-session").prop("checked")
      );
      formDataUpdate.append(
        "ActivitySchedules[" + scheduleCounter + "].SessionName",
        replaceEmptyvalue($(this).find(".session-period").val(), "")
      );
      formDataUpdate.append(
        "ActivitySchedules[" + scheduleCounter + "].StartDate",
        replaceEmptyvalue(
          $(this).find('input[data-class="start-date"]').val(),
          ""
        )
      );

      scheduleCounter++;
    });

    console.log("update activity");

    axios
      .postForm("api/activity/update", formDataUpdate)
      .then((response) => {
        console.log(response);
      })
      .catch((error) => {
        console.log(error);
      });
  } else {
    let counter = 0;
    var imageFile = null;

    $(".img-banner").each(function (e) {
      const name = $(this).attr("data-name");
      if (name) {
        imageFile = dataUrlToFile($(imageData[counter]).val(), name);

        if (imageFile) {
          formData.append(`Image${counter + 1}`, imageFile);
          counter++;
          hasUploadedFile = true;
        }
      }
    });

    if (hasUploadedFile) {
      populateFormData();

      console.log("create activity");

      axios
        .postForm("api/activity/create", formData)
        .then((response) => {
          console.log(response);
        })
        .catch((error) => {
          console.log(error);
        });
    } else {
      fetch("/images/placeholder-image.png")
        .then((response) => {
          response
            .blob()
            .then((blob) => {
              const file = new File([blob], "placeholder-image.png", {
                type: blob.type,
              });

              formData.append("Image1", file);
              formData.append("Image2", file);
              formData.append("Image3", file);

              populateFormData();

              console.log("create activity");

              axios
                .postForm("api/activity/create", formData)
                .then((response) => {
                  console.log(response);
                })
                .catch((error) => {
                  console.log(error);
                });
            })
            .catch((e) => {
              console.error(e);
            });
        })
        .catch((error) => {
          console.log(error);
        });
    }
  }
}

function populateFormData() {
  let scheduleCounter = 0;

  formData.append(
    "ExperienceTypeId",
    $('input[name="experience-type"]:checked').val()
  );
  formData.append(
    "ExperienceCategoryId",
    $("#create-activity-category").val() != 0
      ? $("#create-activity-category").val()
      : 2
  );
  formData.append(
    "SubCategoryId",
    $("#create-activity-sub-category").val() != null
      ? $("#create-activity-sub-category").val()
      : 51
  );
  formData.append(
    "Title",
    replaceEmptyvalue($("#create-activity-title").val(), NOTHING_PROVIDED)
  );
  formData.append(
    "Description",
    replaceEmptyvalue($("#create-activity-description").val(), NOTHING_PROVIDED)
  );
  formData.append("Price", replaceEmptyvalue(null, NOTHING_PROVIDED));
  formData.append(
    "ScheduleIndicator",
    replaceEmptyvalue(null, NOTHING_PROVIDED)
  );
  formData.append("Remarks", replaceEmptyvalue(null, NOTHING_PROVIDED));
  formData.append(
    "IsPublished",
    $("#activity-is-published").val() == "true" ? true : false
  );
  formData.append(
    "Address1",
    replaceEmptyvalue($("#house-number").val(), NOTHING_PROVIDED)
  );
  formData.append("Address2", replaceEmptyvalue(null, NOTHING_PROVIDED));
  formData.append("District", replaceEmptyvalue(null, NOTHING_PROVIDED));
  formData.append("City", replaceEmptyvalue($("#city").val(), ""));
  formData.append("Subdivision", replaceEmptyvalue(null, NOTHING_PROVIDED));
  formData.append("Region", replaceEmptyvalue($("#region").val(), ""));
  formData.append("Barangay", replaceEmptyvalue($("#barangay").val(), ""));
  formData.append(
    "PostalCode",
    replaceEmptyvalue($("#postal-code").val(), NOTHING_PROVIDED)
  );
  formData.append(
    "PinnedLocation",
    replaceEmptyvalue($(".pinned-location").val(), NOTHING_PROVIDED)
  );
  formData.append(
    "SpecificsYouWillProvide",
    replaceEmptyvalue(null, NOTHING_PROVIDED)
  );
  formData.append(
    "CustomerBringWithThem",
    replaceEmptyvalue($(".ql-editor").html(), NOTHING_PROVIDED)
  );
  formData.append(
    "AdditionalRequirements",
    replaceEmptyvalue(null, NOTHING_PROVIDED)
  );
  formData.append("ActivityLevel", "Beginner");
  formData.append("SkillLevel", "No experience");
  formData.append("MinimumAge", 0);
  formData.append("CanAdultsJoin", false);
  formData.append("IsSetSession", false);
  formData.append("SessionName", replaceEmptyvalue(null, NOTHING_PROVIDED));
  formData.append("SearchTags", [NOTHING_PROVIDED]);
  formData.append("Status", 2);

  $(
    "#activitySchedules .accordion-item .accordion-collapse .accordion-body"
  ).each(function () {
    let hasExpiration = 0;

    if ($(this).find("[id^=at-start-]").prop("checked")) hasExpiration = 1;
    else if ($(this).find("[id^=first-attendance-]").prop("checked"))
      hasExpiration = 2;

    formData.append(
      "ActivitySchedules[" + scheduleCounter + "].Name",
      replaceEmptyvalue($(this).find("#schedule-name").val(), NOTHING_PROVIDED)
    );
    formData.append(
      "ActivitySchedules[" + scheduleCounter + "].DateTime",
      replaceEmptyvalue($(this).find("#schedule-time").val(), NOTHING_PROVIDED)
    );
    formData.append(
      "ActivitySchedules[" + scheduleCounter + "].Price",
      $(this).find("#price-input").val()
    );
    formData.append(
      "ActivitySchedules[" + scheduleCounter + "].UnitPrice",
      replaceEmptyvalue("PHP", NOTHING_PROVIDED)
    );
    formData.append("ActivitySchedules[" + scheduleCounter + "].PerUnit1", 1);
    formData.append(
      "ActivitySchedules[" + scheduleCounter + "].PriceUnit1",
      replaceEmptyvalue("Head", NOTHING_PROVIDED)
    );
    formData.append(
      "ActivitySchedules[" + scheduleCounter + "].PerUnit2",
      $(this).find("#unit2-input").val()
    );
    formData.append(
      "ActivitySchedules[" + scheduleCounter + "].PriceUnit2",
      replaceEmptyvalue("Session", NOTHING_PROVIDED)
    );
    formData.append("ActivitySchedules[" + scheduleCounter + "].Order", 0);
    formData.append(
      "ActivitySchedules[" + scheduleCounter + "].IsActiveSchedule",
      $(this).find("#flexSwitchCheckDefault").prop("checked")
    );
    formData.append(
      "ActivitySchedules[" + scheduleCounter + "].HasExpiration",
      hasExpiration
    );
    formData.append(
      "ActivitySchedules[" + scheduleCounter + "].IsSetSession",
      $(this).find(".set-session").prop("checked")
    );
    formData.append(
      "ActivitySchedules[" + scheduleCounter + "].SessionName",
      replaceEmptyvalue($(this).find(".session-period").val(), "")
    );
    formData.append(
      "ActivitySchedules[" + scheduleCounter + "].StartDate",
      replaceEmptyvalue(
        $(this).find('input[data-class="start-date"]').val(),
        "1/1/0001"
      )
    );

    scheduleCounter++;
  });

  $(".pricing-schedule").each(function () {
    let hasExpiration = 0;

    if ($(this).find("[id^=at-start-]").prop("checked")) hasExpiration = 1;
    else if ($(this).find("[id^=first-attendance-]").prop("checked"))
      hasExpiration = 2;

    formData.append(
      "ActivitySchedules[" + scheduleCounter + "].Name",
      replaceEmptyvalue($(this).find("#schedule-name").val(), NOTHING_PROVIDED)
    );
    formData.append(
      "ActivitySchedules[" + scheduleCounter + "].DateTime",
      replaceEmptyvalue($(this).find("#schedule-time").val(), NOTHING_PROVIDED)
    );
    formData.append(
      "ActivitySchedules[" + scheduleCounter + "].Price",
      $(this).find("#price-input").val()
    );
    formData.append(
      "ActivitySchedules[" + scheduleCounter + "].UnitPrice",
      replaceEmptyvalue("PHP", NOTHING_PROVIDED)
    );
    formData.append("ActivitySchedules[" + scheduleCounter + "].PerUnit1", 1);
    formData.append(
      "ActivitySchedules[" + scheduleCounter + "].PriceUnit1",
      replaceEmptyvalue("Head", NOTHING_PROVIDED)
    );
    formData.append(
      "ActivitySchedules[" + scheduleCounter + "].PerUnit2",
      $(this).find("#unit2-input").val()
    );
    formData.append(
      "ActivitySchedules[" + scheduleCounter + "].PriceUnit2",
      replaceEmptyvalue("Session", NOTHING_PROVIDED)
    );
    formData.append("ActivitySchedules[" + scheduleCounter + "].Order", 0);
    formData.append(
      "ActivitySchedules[" + scheduleCounter + "].IsActiveSchedule",
      $(this).find("#flexSwitchCheckDefault").prop("checked")
    );
    formData.append(
      "ActivitySchedules[" + scheduleCounter + "].HasExpiration",
      hasExpiration
    );
    formData.append(
      "ActivitySchedules[" + scheduleCounter + "].IsSetSession",
      $(this).find(".set-session").prop("checked")
    );
    formData.append(
      "ActivitySchedules[" + scheduleCounter + "].SessionName",
      replaceEmptyvalue($(this).find(".session-period").val(), "N/A")
    );
    formData.append(
      "ActivitySchedules[" + scheduleCounter + "].StartDate",
      replaceEmptyvalue(
        $(this).find('input[data-class="start-date"]').val(),
        "1/1/0001"
      )
    );

    scheduleCounter++;
  });
}

function dataUrlToFile(dataUrl, filename) {
  var arr = dataUrl.split(","),
    mime = arr[0].match(/:(.*?);/)[1],
    bstr = atob(arr[1]),
    n = bstr.length,
    u8arr = new Uint8Array(n);

  while (n--) {
    u8arr[n] = bstr.charCodeAt(n);
  }

  return new File([u8arr], filename, { type: mime });
}
