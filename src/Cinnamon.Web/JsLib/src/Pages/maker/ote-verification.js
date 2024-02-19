import { Html5Qrcode } from "html5-qrcode";

class QRVerification {
  constructor(targetId, dotnetObj) {
    this.targetId = targetId;
    this.dotnetObj = dotnetObj;
    this.scanner = new Html5Qrcode(this.targetId);
  }

  startScan() {
    const onScanSuccess = async (decodedText, decodedResult) => {
      console.log(`Code matched = ${decodedText}`, decodedResult);
      this.scanner.pause(true);
      await this.dotnetObj.invokeMethodAsync("VerifyQR", decodedText);
    };

    const config = { fps: 10, qrbox: { width: 250, height: 250 } };

    this.scanner.start({ facingMode: "environment" }, config, onScanSuccess);

    $(`#${this.targetId}`).toggleClass("h-auto");
  }

  stopScan() {
    this.scanner.stop();
    $(`#${this.targetId}`).toggleClass("h-auto");
  }

  resumeScan() {
    this.scanner.resume();
  }

  playSuccessNotification() {
    document.querySelector(".notification-successs").play();
  }

  playErrorNotification() {
    document.querySelector(".notification-error").play();
  }
}

export default {
  createInstance: (targetId, obj) => new QRVerification(targetId, obj),
};
