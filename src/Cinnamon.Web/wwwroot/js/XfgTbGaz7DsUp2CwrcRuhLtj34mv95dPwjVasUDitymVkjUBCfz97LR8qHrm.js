export async function getUser(payload) {
  try {
    const load = await fetch(
      "api/account/1UnCvQdTzi8dUHqKWgZGE1Xf7zqDo7EW99shdKGd2xddj4mZLg9UHJhuuYM3",
      {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(payload),
      }
    );
    const result = load.json();
    return result;
  } catch (error) {
    return { success: false, message: error.message };
  }
}
