<?php
    // 连接数据库
    $data = mysqli_connect("localhost", "root", "123456");

    if(mysqli_connect_errno())
    {
	echo mysqli_connect_error();
	return;
    }

    // 选择数据库
    mysqli_query($data, "set names utf8");
    mysqli_select_db($data, "balloon");

    // 查询数据
    $sql = "SELECT * FROM rank ORDER BY score DESC LIMIT 8";
    $result = mysqli_query($data, $sql) or die("<br>SQL ERROR<br/>");
    $num = mysqli_num_rows($result);

    // 准备数据
    $arr = array();
    
    // 写入数据
    for($i = 0; $i < $num; $i++)
    {
	$row = mysqli_fetch_array($result, MYSQLI_ASSOC);
	$id = $row['id'];
	$username = $row['username'];
	$score = $row['score'];
	
	$arr[$id]['id'] = $id;
	$arr[$id]['username'] = $username;
	$arr[$id]['score'] = $score;
    }

    // 释放数据
    mysqli_free_result($result);

    // 关闭数据库
    mysqli_close($data);

    // 发送Json格式的数据
    echo json_encode($arr);
?>
