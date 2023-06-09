const messages = {};
let dotnetObj = undefined;
let messageElementId = undefined;
let chatroomListItem = ".chatroom-list-item";
let sendMessageId = "#send-message";

messages.initControls = (obj, elementId) => {
    dotnetObj = obj;
    messageElementId = elementId;

    $(document).on("keyup", messageElementId, function (e) {
        if (e.which === 13 && !e.shiftKey) {
            dotnetObj.invokeMethodAsync('SendMessageAsync', $(messageElementId).text());
            $(messageElementId).text("")
        }
    });

    $(document).on("click", chatroomListItem, function (e) {
        $(messageElementId).focus();
    });

    $(document).on("click", messageElementId, function (e) {
        dotnetObj.invokeMethodAsync('OnFocus');
    });

    $(document).on("click", sendMessageId, function (e) {
        dotnetObj.invokeMethodAsync('SendMessageAsync', $(messageElementId).text());
        $(messageElementId).text("")
    });

    $(document).ready(function () {
        $("body").tooltip({ selector: '[data-toggle=tooltip]' });
    });
}

function formatDay(date) {
    var dayOfWeek = date.getDay();
    var dayOfWeekString = "Sunday";
    switch (dayOfWeek) {
        case 0:
            dayOfWeekString = "Sunday";
            break;
        case 1:
            dayOfWeekString = "Monday";
            break;
        case 2:
            dayOfWeekString = "Tuesday";
            break;
        case 3:
            dayOfWeekString = "Wednesday";
            break;
        case 4:
            dayOfWeekString = "Thursday";
            break;
        case 5:
            dayOfWeekString = "Friday";
            break;
        case 6:
            dayOfWeekString = "Saturday";
            break;
    }
    return dayOfWeekString;
}

function formatDate(date) {
    var months = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];

    var day = date.getDate();
    var month = months[date.getMonth()];
    var year = date.getFullYear();

    return month + " " + day + ", " + year;
}

export async function handleDateFormat() {
    $(document).ready(function () {
        let lastSeen = $("#last-seen-date").attr("data-last-seen-date");

        if (lastSeen) {
            let lastSeenDate = new Date(`${lastSeen} UTC`);
            $("#last-seen-date").html(`Last seen ${lastSeenDate.toLocaleTimeString([], { timeStyle: 'short' }) }`)
        }

        $(".chatroom-list-item ").each(function () {
            var date = new Date(`${$(this).find('#date-created').val()} UTC`).toLocaleTimeString([], { timeStyle: 'short' });

            $(this).find('.last-update-date').html(date);
        });

        $("#chat-history-list li.list-item-days").each(function () {
            var date = new Date(`${$(this).find('#chat-history-date-created').val()} UTC`);

            var formattedDate = `${formatDate(date)} ${date.toLocaleTimeString([], { timeStyle: 'short' })}`;

            $(this).find('.chat-date-time').html(formattedDate);
        });

        $("#chat-history-list li.list-item-minutes").each(function () {
            var date = new Date(`${$(this).find('#chat-history-date-created').val()} UTC`);

            var formattedDay = `${formatDay(date)}, ${date.toLocaleTimeString([], { timeStyle: 'short' })}`;

            $(this).find('.chat-date-time').html(formattedDay);
        });

        $("#chat-history-list li").each(function () {
            var date = new Date(`${$(this).find('#chat-history-date-created').val()} UTC`);

            $(this).find('[data-toggle=tooltip]').attr("title", date.toLocaleTimeString([], { timeStyle: 'short' }));
        });
    });
};

export async function initMessages(obj, elementId) {
    return await messages.initControls(obj, elementId);
}

export default {
    initMessages,
    handleDateFormat
};
