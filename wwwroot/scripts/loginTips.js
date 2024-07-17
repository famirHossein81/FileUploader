(function newFact() {
  var hints = [
    "Did you know you can upload files in seconds?<br /> Simply drag and drop files from your computer to get started!",
    "Did you know you can upload multiple files at once?<br /> Save time by uploading entire folders or selecting multiple files in one go!",
    "Did you know you can keep your files organized?<br /> Create folders and categories to easily manage and find your files!",
    "Did you know you can easily rename your files?<br /> Keep your files organized and easy to identify with custom file names!"
  ];
  var randomFact = Math.floor(Math.random() * hints.length);
  document.getElementById("login_tips").innerHTML = hints[randomFact];
})();
