var isAdvancedUpload = (function () {
  var div = document.createElement("div");
  return (
    ("draggable" in div || ("ondragstart" in div && "ondrop" in div)) &&
    "FormData" in window &&
    "FileReader" in window
  );
})();

let draggableFileArea = document.querySelector(".drag-file-area");
let browseFileText = document.querySelector(".browse-files");
let uploadIcon = document.querySelector(".upload-icon");
let dragDropText = document.querySelector(".dynamic-message");
let fileInput = document.querySelector(".default-file-input");
let cannotUploadMessage = document.querySelector(".cannot-upload-message");
let cancelAlertButton = document.querySelector(".cancel-alert-button");
let uploadedFile = document.getElementById("file_block");
let fileName = document.querySelector(".file-name");
let fileSize = document.querySelector(".file-size");
let removeFileButton = document.querySelector(".remove-file-icon");
let uploadButton = document.querySelector(".upload-button");
let shareButton = document.getElementById("share-icon");

fileInput.addEventListener("click", () => {
  fileInput.value = "";
});

fileInput.addEventListener("change", (e) => {
  uploadIcon.innerHTML = "FilesUp";
  //dragDropText.innerHTML = "File Dropped Successfully!";
  //document.querySelector(".label").innerHTML = `drag & drop or <span class="browse-files"> <input type="file" class="default-file-input" style=""/> <span class="browse-files-text" style="top: 0;"> browse file</span></span>`;
  uploadButton.innerHTML = `Upload`;
  fileName.innerHTML = fileInput.files[0].name;
  fileSize.innerHTML = (fileInput.files[0].size / 1024).toFixed(1) + " KB";
  uploadedFile.style.cssText = "display: flex;";
});

shareButton.addEventListener("click", async (event) => {
  let link = event.target.getAttribute("data-share-link");
  try {
    await navigator.clipboard.writeText(link);
    alert('Copied to clipboard!');
  } catch (error) {
    alert('Failed to copy: ' + error);
  }
});



const share = async (event) => {
  let link = document.getElementById("download-link");
  link = link.getAttribute("href");
  try {
    await navigator.clipboard.writeText(link);
    alert('Copied to clipboard!');
  } catch (error) {
    alert('Failed to copy: ' + error);
  }
}





/* uploadButton.addEventListener("click", () => {
  if (fileInput.value !== "") {
    uploadButton.innerHTML = `Uploading...`;
    // Create FormData object
    let formData = new FormData(document.getElementById('form-container'));
    
    // Send AJAX request
    fetch('/Home/Upload', {
      method: 'POST',
      body: formData
    })
    .then(response => response.json())
    .then(data => {
      console.log('Success:', data);
      uploadButton.innerHTML = `Uploaded`;
    })
    .catch((error) => {
      console.error('Error:', error);
      uploadButton.innerHTML = `Upload Failed`;
    });
  } else {
    cannotUploadMessage.style.cssText = "display: flex;";
  }
});

cancelAlertButton.addEventListener("click", () => {
  cannotUploadMessage.style.cssText = "display: none;";
});

if (isAdvancedUpload) {
  ["drag", "dragstart", "dragend", "dragover", "dragenter", "dragleave", "drop"].forEach((evt) =>
    draggableFileArea.addEventListener(evt, (e) => {
      e.preventDefault();
      e.stopPropagation();
    })
  );

  ["dragover", "dragenter"].forEach((evt) => {
    draggableFileArea.addEventListener(evt, (e) => {
      e.preventDefault();
      e.stopPropagation();
      dragDropText.innerHTML = "Drop your file here!";
    });
  });

  draggableFileArea.addEventListener("drop", (e) => {
    dragDropText.innerHTML = "File Dropped Successfully!";
    document.querySelector(".label").innerHTML = `drag & drop or <span class="browse-files"> <input type="file" class="default-file-input" style=""/> <span class="browse-files-text" style="top: -23px; left: -20px;"> browse file</span> </span>`;
    uploadButton.innerHTML = `Upload`;

    let files = e.dataTransfer.files;
    fileInput.files = files;
    fileName.innerHTML = files[0].name;
    fileSize.innerHTML = (files[0].size / 1024).toFixed(1) + " KB";
    uploadedFile.style.cssText = "display: flex;";
  });
}

removeFileButton.addEventListener("click", () => {
  uploadedFile.style.cssText = "display: none;";
  fileInput.value = "";
  dragDropText.innerHTML = "Drag & drop any file here";
  document.querySelector(".label").innerHTML = `or <span class="browse-files"> <input type="file" class="default-file-input"/> <span class="browse-files-text">browse file</span> <span>from device</span> </span>`;
  uploadButton.innerHTML = `Upload`;
}); */