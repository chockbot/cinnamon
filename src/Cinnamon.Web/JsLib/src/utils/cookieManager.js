import cookies from "js-cookie";

const cookieManager = {};

cookieManager.setCookie = (key, value, opts) => {
  cookies.set(key, value, opts);
};

cookieManager.getCookie = (key, opts) => {
  const value = cookies.get(key, opts);
  return { success: true, message: value };
};

cookieManager.removeCookie = (key, opts) => {
  cookies.remove(key, opts);
};

export default cookieManager;
