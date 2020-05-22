<?php
include 'db.php';

// Unity sends the form data as POST
$checkUnity = $_POST['game'];
$email = $_POST['email'];
$upass = $_POST['password'];
$username = $_POST['username'];



if ($email != null) {
// clean so users cannot inject code to server!

$email = strip_tags($email);
$password = strip_tags($upass);
$username = strip_tags($username);


// encrypt passw

$password = hash('md5', $password);


if ($checkUnity == "BackToThe80s") {
// Make the query of email and password

$query = "SELECT email, name FROM hscores WHERE email='$email' OR name='$username' ";

$result = mysqli_query($connection, $query);



// check if user exists
$row = mysqli_fetch_row($result);

if ($row) {

    $dataArray = array('success' => false, 'error' => 'ERROR: user already exists!');

}else {

    // insert new user!
    $query2 = "INSERT INTO hscores (name,email,password) VALUES ('$username','$email','$password')";

    // was this success
    if($result2 = mysqli_query($connection, $query2)) {

        $id = mysqli_insert_id($connection);

        $dataArray = array('success' => true, error => ' ', 'email' => "$email", 'id' => $id);

    } else {

        $dataArray = array('success' => false, 'error' => 'ERROR: Cannot create user!');

    }

}









}

else {

    $dataArray = array('success' => false, 'error' => 'Error: Email is null!');

}

} else {
    Die("ERROR! No Access!");
}



//return the data for Unity...

header('Content-Type: application/json');

echo json_encode($dataArray);



?>