<?php 
/*
ini_set('display_errors', 1);
ini_set('display_startup_errors', 1);
error_reporting(E_ALL);
*/


$connection = mysqli_connect('localhost', 'harriaho_bttUseR', 'xYx9!23zAQWeR', 'harriaho_btt80');

if ($connection){
    //echo "DB CONNECTED!!!";
} else {

    echo "DB NOT CONNECTED!";
    //$successMessage = "DB VIRHE!!!";
    die("ERROR! VIRHE! Ei yhteyttä tietokantaan! Tarkista internetyhteys!");

} 

?>