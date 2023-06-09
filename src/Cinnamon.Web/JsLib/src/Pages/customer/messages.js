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
}

export async function initMessages(obj, elementId) {
    return await messages.initControls(obj, elementId);
}

export default {
    initMessages,
};
