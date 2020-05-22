<?php
include("db.php");

//echo "Debug1";

$checkUnity = $_POST['game'];

$connection->set_charset("utf8");


if ($checkUnity == "BackToThe80s") {
    //$query = "SELECT * FROM hscores WHERE email = '$email' ";

// Get the highscores and sort them by score so don't need to do nothing in Unity.
$query = "SELECT id,name,score FROM hscores ORDER BY score DESC LIMIT 10";

$result = mysqli_query($connection, $query);

if(!$result) {

    echo("Error description: " . mysqli_error($connection));

    die("Error!");



} else {



    while ($row = mysqli_fetch_assoc($result)) {

            

            $rows[] = $row;

        

    } 



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
echo json_encode(utf8ize(array("highscores" => $rows)));
?>