const editInfo = () => {
  const edit_info = document.getElementById("edit_info")
  navOut();
  if(edit_info.style.display === "none"){
    edit_info.style.display = "flex"
  } else {
    edit_info.style.display = "none"
  }
}