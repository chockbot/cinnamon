import exceljs from "exceljs";
import fileSaver from "file-saver";

const experienceExcelWriter = {};

experienceExcelWriter.execute = async (data) => {
  const workbook = new exceljs.Workbook();
  const sheet = workbook.addWorksheet("activities");

  // add headers
  const headerRow = sheet.getRow(1);
  headerRow.getCell(1).value = "Email";
  headerRow.getCell(2).value = "First Name";
  headerRow.getCell(3).value = "Last Name";
  headerRow.getCell(4).value = "Title";
  headerRow.getCell(5).value = "Deactivated";


  // activities
  for (let i = 0; i < data.length; i++) {
    const obj = data[i];
    const row = sheet.getRow(i + 2);
    row.getCell(1).value = obj.owner.email;
    row.getCell(2).value = obj.owner.firstName;
    row.getCell(3).value = obj.owner.lastName;
    row.getCell(4).value = obj.title;
    row.getCell(5).value = obj.isPublished ? "No" : "Yes";
  
  }

  const buffer = await workbook.xlsx.writeBuffer();
  fileSaver.saveAs(new Blob([buffer]), "Experience.xlsx");
};

export default experienceExcelWriter;
