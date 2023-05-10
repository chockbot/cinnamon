const NOTHING_PROVIDED = "--NOTHING PROVIDED--";
const formData = new FormData();
var timeout;
var isToggled = false;

export function confirmation() {

    $(document).ready(function () {
        
        if (window.history && window.history.pushState) {

            window.history.pushState('forward', null, window.location.href);

            $(window).on('popstate', function () {
                if (confirm('Are you sure you want to leave this page?')) {

                    if (!isToggled) {
                        isToggled = true;

                        var activityId = $("#activity-id").val();
                        var schedules = [];

                        if (activityId && activityId != "0") {
                            var data = {
                                "ExperienceTypeId": $('input[name="experience-type"]:checked').val(),
                                "ExperienceCategoryId": $('#create-activity-category').val(),
                                "SubCategoryId": $('#create-activity-sub-category').val(),
                                "Title": replaceEmptyvalue($("#create-activity-title").val(), NOTHING_PROVIDED),
                                "Description": replaceEmptyvalue($("#create-activity-description").val(), NOTHING_PROVIDED),
                                "Price": replaceEmptyvalue(null, NOTHING_PROVIDED),
                                "ScheduleIndicator": replaceEmptyvalue(null, NOTHING_PROVIDED),
                                "Remarks": replaceEmptyvalue(null, NOTHING_PROVIDED),
                                "IsPublished": $('#activity-is-published').val() == 'true' ? true : false,
                                "Address1": replaceEmptyvalue($("#house-number").val(), NOTHING_PROVIDED),
                                "Address2": replaceEmptyvalue(null, NOTHING_PROVIDED),
                                "District": replaceEmptyvalue(null, NOTHING_PROVIDED),
                                "City": replaceEmptyvalue($("#city").val(), ""),
                                "Subdivision": replaceEmptyvalue(null, NOTHING_PROVIDED),
                                "Region": replaceEmptyvalue($("#region").val(), ""),
                                "Barangay": replaceEmptyvalue($("#barangay").val(), ""),
                                "PostalCode": replaceEmptyvalue($("#postal-code").val(), NOTHING_PROVIDED),
                                "PinnedLocation": replaceEmptyvalue($(".pinned-location").val(), NOTHING_PROVIDED),
                                "SpecificsYouWillProvide": replaceEmptyvalue(null, NOTHING_PROVIDED),
                                "CustomerBringWithThem": replaceEmptyvalue(null, NOTHING_PROVIDED),
                                "AdditionalRequirements": replaceEmptyvalue(null, NOTHING_PROVIDED),
                                "ActivityLevel": "Beginner",
                                "SkillLevel": "No experience",
                                "MinimumAge": 0,
                                "CanAdultsJoin": false,
                                "IsSetSession": false,
                                "SessionName": replaceEmptyvalue(null, NOTHING_PROVIDED),
                                "SearchTags": [NOTHING_PROVIDED],
                                "Status": parseInt($("#activity-status").val()),
                                "ActivityId": activityId
                            };

                            $('#activitySchedules .accordion-item .accordion-collapse .accordion-body').each(function () {
                                schedules.push({
                                    "Id": $(this).find('#schedule-id').val(),
                                    "Name": replaceEmptyvalue($(this).find('#schedule-name').val(), NOTHING_PROVIDED),
                                    "DateTime": replaceEmptyvalue($(this).find('#schedule-time').val(), NOTHING_PROVIDED),
                                    "Price": $(this).find('#price-input').val(),
                                    "UnitPrice": replaceEmptyvalue("PHP", NOTHING_PROVIDED),
                                    "PerUnit1": 1,
                                    "PriceUnit1": replaceEmptyvalue("Head", NOTHING_PROVIDED),
                                    "PerUnit2": $(this).find('#unit2-input').val(),
                                    "PriceUnit2": replaceEmptyvalue("Session", NOTHING_PROVIDED),
                                    "Order": 0,
                                    "IsActiveSchedule": $(this).find('#flexSwitchCheckDefault').prop('checked')
                                });
                            });

                            $('.pricing-schedule').each(function () {

                                var isValid = ($(this).find('#schedule-name').val() && $(this).find('#schedule-time').val() && $(this).find('#price-input').val() > 0) ? true : false;

                                if (isValid) {
                                    schedules.push({
                                        "Name": replaceEmptyvalue($(this).find('#schedule-name').val(), NOTHING_PROVIDED),
                                        "DateTime": replaceEmptyvalue($(this).find('#schedule-time').val(), NOTHING_PROVIDED),
                                        "Price": $(this).find('#price-input').val(),
                                        "UnitPrice": replaceEmptyvalue("PHP", NOTHING_PROVIDED),
                                        "PerUnit1": 1,
                                        "PriceUnit1": replaceEmptyvalue("Head", NOTHING_PROVIDED),
                                        "PerUnit2": $(this).find('#unit2-input').val(),
                                        "PriceUnit2": replaceEmptyvalue("Session", NOTHING_PROVIDED),
                                        "Order": 0,
                                        "IsActiveSchedule": $(this).find('#flexSwitchCheckDefault').prop('checked')
                                    });
                                }
                            });

                            data.activitySchedules = schedules;

                            console.log('update activity');

                            axios.post("api/activity/update", data)
                                .then(response => {
                                    console.log(response);
                                })
                                .catch(error => {
                                    console.log(error);
                                });
                        }
                        else {

                            fetch('/images/placeholder-image.png')
                                .then(response => {
                                    response.blob()
                                        .then(blob => {
                                            const file = new File([blob], 'placeholder-image.png', { type: blob.type });

                                            formData.append("Image1", file);
                                            formData.append("Image2", file);
                                            formData.append("Image3", file);
                                            formData.append("ExperienceTypeId", 1);
                                            formData.append("ExperienceCategoryId", 2);
                                            formData.append("SubCategoryId", 51);
                                            formData.append("Title", replaceEmptyvalue($("#create-activity-title").val(), NOTHING_PROVIDED));
                                            formData.append("Description", replaceEmptyvalue($("#create-activity-description").val(), NOTHING_PROVIDED));
                                            formData.append("Price", replaceEmptyvalue(null, NOTHING_PROVIDED));
                                            formData.append("ScheduleIndicator", replaceEmptyvalue(null, NOTHING_PROVIDED));
                                            formData.append("Remarks", replaceEmptyvalue(null, NOTHING_PROVIDED));
                                            formData.append("IsPublished", true);
                                            formData.append("Address1", replaceEmptyvalue($("#house-number").val(), NOTHING_PROVIDED));
                                            formData.append("Address2", replaceEmptyvalue(null, NOTHING_PROVIDED));
                                            formData.append("District", replaceEmptyvalue(null, NOTHING_PROVIDED));
                                            formData.append("City", replaceEmptyvalue($("#city").val(), ""));
                                            formData.append("Subdivision", replaceEmptyvalue(null, NOTHING_PROVIDED));
                                            formData.append("Region", replaceEmptyvalue($("#region").val(), ""));
                                            formData.append("Barangay", replaceEmptyvalue($("#barangay").val(), ""));
                                            formData.append("PostalCode", replaceEmptyvalue($("#postal-code").val(), NOTHING_PROVIDED));
                                            formData.append("PinnedLocation", replaceEmptyvalue($(".pinned-location").val(), NOTHING_PROVIDED));
                                            formData.append("SpecificsYouWillProvide", replaceEmptyvalue(null, NOTHING_PROVIDED));
                                            formData.append("CustomerBringWithThem", replaceEmptyvalue(null, NOTHING_PROVIDED));
                                            formData.append("AdditionalRequirements", replaceEmptyvalue(null, NOTHING_PROVIDED));
                                            formData.append("ActivityLevel", "Beginner");
                                            formData.append("SkillLevel", "No experience");
                                            formData.append("MinimumAge", 0);
                                            formData.append("CanAdultsJoin", false);
                                            formData.append("IsSetSession", false);
                                            formData.append("SessionName", replaceEmptyvalue(null, NOTHING_PROVIDED));
                                            formData.append("SearchTags", [NOTHING_PROVIDED]);

                                            for (var i = 0; i < 1; i++) {
                                                formData.append("ActivitySchedules[" + i + "].Name", replaceEmptyvalue(null, NOTHING_PROVIDED));
                                                formData.append("ActivitySchedules[" + i + "].DateTime", replaceEmptyvalue(null, NOTHING_PROVIDED));
                                                formData.append("ActivitySchedules[" + i + "].Price", 0.0);
                                                formData.append("ActivitySchedules[" + i + "].UnitPrice", replaceEmptyvalue("PHP", NOTHING_PROVIDED));
                                                formData.append("ActivitySchedules[" + i + "].PerUnit1", 1);
                                                formData.append("ActivitySchedules[" + i + "].PriceUnit1", replaceEmptyvalue("Head", NOTHING_PROVIDED));
                                                formData.append("ActivitySchedules[" + i + "].PerUnit2", 1);
                                                formData.append("ActivitySchedules[" + i + "].PriceUnit2", replaceEmptyvalue("Session", NOTHING_PROVIDED));
                                                formData.append("ActivitySchedules[" + i + "].Order", 0);
                                                formData.append("ActivitySchedules[" + i + "].IsActiveSchedule", true);
                                            }

                                            formData.append("Status", 2);
                                            console.log('create activity');
                                            axios.postForm("api/activity/create", formData)
                                                .then(response => {
                                                    console.log(response);
                                                })
                                                .catch(error => {
                                                    console.log(error);
                                                });
                                        }).catch(e => {
                                            console.error(e);
                                        })
                                })
                                .catch(error => {
                                    console.log(error);
                                });
                        }

                        setTimeout(redirectToDashboard, 3000);
                    }

                } else {
                    console.log('stayed on page');
                    history.pushState(null, null, window.location.pathname);
                    event.preventDefault();
                }
            });

        }

        $("#regForm").on("mousedown", stopNavigate);

        $("#regForm").on("mouseleave", function () {
      
        });
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

function replaceEmptyvalue(value, replacement) {
    return value ? value : replacement;
}

function redirectToDashboard() {
    window.location.href = window.location.origin + "/maker/dashboard";
}

