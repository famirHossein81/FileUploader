const showPass = () => {
  const password = document.getElementById("password");
  const cpassword = document.getElementById("cpassword");
  if (password.type === "password") {
    password.type = "text";
    cpassword.type = "text";
  } else {
    password.type = "password";
    cpassword.type = "password";
  }
};

const terms = () => {
  const signup_submit = document.getElementById("signup_submit")
  if (signup_submit.disabled === true) {
    signup_submit.disabled = false;
  } else {
    signup_submit.disabled = true;
  }
};

const forgotPass = () => {
  const forgot_pass_form = document.getElementById("forgot_pass_bg")
  if(forgot_pass_form.style.display === "none"){
    forgot_pass_form.style.display = "block"
  } else {
    forgot_pass_form.style.display = "none"
  }
}
