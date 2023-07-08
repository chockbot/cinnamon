import exceljs from "exceljs";
import fileSaver from "file-saver";

const excelWriter = {};

excelWriter.execute = async (data) => {
  const workbook = new exceljs.Workbook();
  const sheet = workbook.addWorksheet("transactions");

  // add headers
  const headerRow = sheet.getRow(1);
  headerRow.getCell(1).value = "First Name";
  headerRow.getCell(2).value = "Last Name";
  headerRow.getCell(3).value = "Email";
  headerRow.getCell(4).value = "Total";
  headerRow.getCell(5).value = "Convenience Fee";
  headerRow.getCell(6).value = "Credit Amount";
  headerRow.getCell(7).value = "Overall Total";
  headerRow.getCell(8).value = "Status";
  headerRow.getCell(9).value = "Date Purchased";

  // transactions
  for (let i = 0; i < data.length; i++) {
    const obj = data[i];
    const row = sheet.getRow(i + 2);
    row.getCell(1).value = obj.provider.firstName;
    row.getCell(2).value = obj.provider.lastName;
    row.getCell(3).value = obj.provider.email;
    row.getCell(4).value = obj.total;
    row.getCell(5).value = obj.convinienceFee;
    row.getCell(6).value = obj.creditAmount;
    row.getCell(7).value = obj.overallTotal;
    row.getCell(8).value = obj.status;
    row.getCell(9).value = obj.purchaseDate.toString();
  }

  const buffer = await workbook.xlsx.writeBuffer();
  fileSaver.saveAs(new Blob([buffer]), "result.xlsx");
};

export default excelWriter;
