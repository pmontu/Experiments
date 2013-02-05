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
		$contents = file_get_contents($raw);
		return htmlentities($contents,ENT_IGNORE, "UTF-8");
		//."<br><a href='$url'>$name</a>";
	}
}
add_shortcode( 'github', 'github_func' );

function collapse_func( $atts, $contents ) {
	extract($atts);
	return "<a href='' class='collapsible'>$name<span></span></a>"
	."<div>"
	.do_shortcode($contents)
	."</div>";
}
add_shortcode( 'collapse', 'collapse_func' );

function my_scripts_method() {
	wp_enqueue_script(
		'collapse',
		plugins_url('collapse.js',__FILE__),
		array('jquery')
	);
	wp_enqueue_script(
		'jquery.collapsible',
		plugins_url('jquery.collapsible.js',__FILE__),
		array('jquery')
	);
	wp_register_style( 
		'collapse_css', 
		plugins_url( 'collapse.css', __FILE__ ),
		array(),
		'0',
		'all'
	);
	wp_enqueue_style( 'collapse_css');
 }
add_action('wp_enqueue_scripts', 'my_scripts_method');
?>
