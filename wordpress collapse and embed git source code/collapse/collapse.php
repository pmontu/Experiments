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

// This just replaces the bad word with censored, we'll style it later
function collapse($content) {
    $profanities = array('badword','alsobad','...');
    $content=str_ireplace($profanities,'{censored}',$content);
    return $content;
}

// Now we set that function up to execute when the the_content filter is called
add_filter( 'the_content', 'collapse' );

// We need some CSS to position the paragraph
function collapse_css() {

	echo "
	<style type='text/css'>
	#dolly {
	}
	</style>
	";
}

add_action( 'admin_head', 'collapse_css' );

function collapse_js() {
    echo '<script type="text/javascript" src="'.plugins_url('collapse.js',__FILE__).'"></script>';
}

// Add hook for front-end <head></head>
add_action('wp_head', 'collapse_js');

?>
