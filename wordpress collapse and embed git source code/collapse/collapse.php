<?php
/**
 * @package Collapse
 * @version 0
 */
/*
Plugin Name: Collapse
Description: Collapse
Author: Manojkumar Purushothaman
Version: 0
Author URI: http://pmontu.com/
*/

// [bartag foo="foo-value"]
function github_func( $atts ) {
	extract($atts);
	$url = trim($file);
	$raw = str_replace(array("https://","/blob/"),array("https://raw.","/"),$url);
	if (strlen($raw) > 0){
		return file_get_contents($raw)."<br><a href='$url'>$name</a>";
	}
}
add_shortcode( 'github', 'github_func' );

?>
