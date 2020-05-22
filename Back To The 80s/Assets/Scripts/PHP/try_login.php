<?php
include 'db.php';

// Unity sends the form data as POST
$checkUnity = $_POST['game'];

//$connection->set_charset("utf8");
$email = $_POST['email'];
$postpass = $_POST['password'];

// clean the inputs
$email = strip_tags($email);
$password = strip_tags($postpass);

// encrypt the password
$password = hash('md5', $password);

if ($checkUnity == "BackToThe80s") {
// Query the email and password
$query = "SELECT * FROM hscores WHERE email='$email' AND password='$password'";
$result = mysqli_query($connection, $query);

// fetch the data!
$row = mysqli_fetch_row($result);
if ($row) {
    // Get the email, id and level the level row is 2 because 
    // up you can see the SELECT userEmail, userId, userLevel FROM... and it is the 3rd (0,1,2)
    
    
    $dataArray = array('success' => true, 'error' => ' ', 'email' => "$email", 'id' => $row[0], 'score' => $row[2], 'username' => $row[1]);

    /*
    while ($row = mysqli_fetch_assoc($result)) {

            

        $rows[] = $row;

    

    } 
    */


}else {
    $dataArray = array('success' => false, 'error' => 'ERROR: wrong email or password', 'email' => "");
}

} else {
    Die("ERROR! No Access!");
}

function utf8ize($d) {
    if (is_array($d)) {
        foreach ($d as $k => $v) {
            $d[$k] = utf8ize($v);
        }
    } else if (is_string ($d)) {
        return utf8_encode($d);
    }
    return $d;
}

header('Content-Type: application/json');
echo json_encode($dataArray);

/*
header('Content-Type: application/json');
echo json_encode(utf8ize(array("User" => $rows)));
*/
?>