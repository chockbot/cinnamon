import axios from "axios";

const externalRegister = {};

externalRegister.init = async (obj) => {
    try {
        const payload = {
            firstname: obj.firstname,
            lastname: obj.lastname,
            email: obj.email,
            birthdate: obj.birthdate,
            phonenumber: obj.phonenumber,
            password: obj.password,
            token: obj.token,
            guid: obj.guid,
            hasAcceptedTerms: obj.hasAcceptedTerms
        };

        const { data } = await axios.post(
            "/api/account/externalregisterautologin",
            payload
        );

        return data;
    } catch {
        return {
            success: false,
            message: "An error occured. Please try again later.",
        };
    }
};

export default externalRegister;
