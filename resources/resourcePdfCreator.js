import jsPDF from "jspdf";
import "jspdf-autotable";
import * as resourcesPdfService from "../../services/resourcesPdfService";
let exportPDF = (resources, email, check) => {
  const unit = "pt";
  const size = "A4"; // Use A1, A2, A3 or A4
  const orientation = "landscape"; // portrait or landscape
  const doc = new jsPDF(orientation, unit, size);
  doc.setFontSize(12);
  const headers = [["BUSINESS", "CONTACT NAME", "EMAIL", "PHONE", "SITE URL"]];
  const data = resources.map(rec => [
    rec.name,
    rec.contactName,
    rec.contactEmail,
    rec.phone,
    rec.siteUrl
  ]);

  let content = {
    startY: 70,
    head: headers,
    body: data
  };

  var img = new Image();
  img.src = "https://i.imgur.com/4JE4xYL.png";
  doc.addImage(img, "PNG", 655, 15, 140, 55);

  doc.autoTable(content);

  if (check === true) {
    var blob = doc.output("blob");
    var formData = new FormData();
    formData.append("file", blob);

    resourcesPdfService.uploadPdf(formData, email);
  } else {
    doc.save("Resources.pdf");
  }
};

export { exportPDF };
