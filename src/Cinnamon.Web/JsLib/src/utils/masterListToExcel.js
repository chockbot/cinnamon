import exceljs from "exceljs";
import fileSaver from "file-saver";

const masterListToExcel = {};

masterListToExcel.execute = async (enrollees) => {
    const workbook = new exceljs.Workbook();
    const sheet = workbook.addWorksheet("masterlist");

    // add headers
    const headerRow = sheet.getRow(1);
    headerRow.getCell(1).value = "Name";
    headerRow.getCell(2).value = "Age";
    headerRow.getCell(3).value = "Gender";
    headerRow.getCell(4).value = "Sessions Attended";
    headerRow.getCell(5).value = "Last Activity Purchased";
    headerRow.getCell(6).value = "Email contact of enrollee";
    headerRow.getCell(7).value = "Student no/Jersey No.";
    headerRow.getCell(8).value = "Remarks";

    // master list
    for (let i = 0; i < enrollees.length; i++) {
        const obj = enrollees[i];
        const row = sheet.getRow(i + 2);
        row.getCell(1).value = obj.Name; // Use uppercase property names
        row.getCell(2).value = obj.Age;
        row.getCell(3).value = obj.Gender;
        row.getCell(4).value = obj.SessionsAttended;
        row.getCell(5).value = obj.ActivityName;
        row.getCell(6).value = obj.Email;
        row.getCell(7).value = obj.StudentNo;
        row.getCell(8).value = obj.Remarks;
    }

    // Generate the file name with "masterlist" and the current date
    const timestamp = new Date().toISOString().replace(/[:.]/g, "-");
    fileSaver.saveAs(new Blob([await workbook.xlsx.writeBuffer()]), `masterlist_${timestamp}.xlsx`);
};

export default masterListToExcel;
