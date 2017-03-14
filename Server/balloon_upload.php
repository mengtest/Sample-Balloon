<?php
    // 获取用户名和分数
    $username = $_POST["username"];
    $score = $_POST["score"];

    // 连接数据库
    $data = mysqli_connect("localhost", "root", "123456");
    if(mysqli_connect_errno())
    {
	echo mysqli_connect_error();
	return;
    }

    // 检查用户名是否合法（防止SQL注入）
    $username = mysqli_real_escape_string($data, $username);

    // 选择数据库
    mysqli_query($data, "set names utf8");
    mysqli_select_db($data, "balloon");

    // 插入新数据
    $sql = "INSERT INTO rank VALUES(NULL, '$username', '$score')";
    mysqli_query($data, $sql);

    // 关闭数据库
    mysqli_close($data);

    echo 'upload -> '.$username." : ".$score;
?>
