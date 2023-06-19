const inputTypeChanger = {};

inputTypeChanger.execute = (selector, type) => {
  const allowedTypes = {
    text: "text",
    password: "password",
    number: "number",
  };

  if (!allowedTypes[type]) return;

  $(selector).attr("type", type);
};

export default inputTypeChanger;
