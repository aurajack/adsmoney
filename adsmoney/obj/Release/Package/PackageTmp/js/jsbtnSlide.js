/**
 *  a jQuery plugin for creating simple, user-friendly, 508-compliant,
 *  iPhone-style slider-toggles using a mix of HTML, CSS, and Javascript.
 *
 *  @author rees.byars
 */ (function ($) {

     var sliderTogglePluginName = "sliderToggle";

     /**
      * values for keycodes, mainly for use in 508
      */
     var keyCodes = {
         tab: 9,
         enter: 13,
         space: 32,
         left: 37,
         up: 38,
         right: 39,
         down: 40
     };

     var defaults = {
         height: 28,
         width: 78,
         ballwidth: 18,
         tabindex: 0,
         speed: 300
     };

     function SliderToggle(element, options) {
         this.element = element;
         this.options = $.extend({}, defaults, options);
         this.options = $.extend({}, this.options, $(element).data());
         this.init();
     }

     SliderToggle.prototype = {

         init: function () {

             var $container = $(this.element);
             var $choices = $container.children('input[type=radio]');

             if ($choices.length != 2) {
                 alert("Error:  the slider toggle container [" + $container.attr('id') + "] must contain exactly 2 radio inputs");
                 return;
             }

             // get the two radio options and hide them since we are "replacing" them with our own
             var $leftRadio = $($choices[0]).hide();
             var $rightRadio = $($choices[1]).hide();

             // get the text to display on the left and right sides of the slider, use label if present, use radio value otherwise
             function getText($radio) {
                 var $label = $('label[for="' + $radio.attr('id') + '"]');
                 if ($label.length <= 0) {
                     return $radio.val();
                 }
                 $label.hide();
                 return $label.text();
             }
             var onText = getText($leftRadio);
             var offText = getText($rightRadio);

             /*
              setup rest the values to be used
              */

             var height = this.options.height;
             var width = this.options.width;
             var ballWidth = this.options.ballwidth;
             var ballRight = width - ballWidth - 2; //2 due to the border
             var hideWidth = ballWidth / 2;
             var showWidth = width - hideWidth;
             var speed = this.options.speed;
             var tabIndex = $leftRadio.attr('tabindex');
             if (!tabIndex) {
                 tabIndex = this.options.tabindex;
             }

             /*
              style and/or create all the elements
              */

             var $label = $container.children('.slider-toggle-label-text')
                 .css({
                     'line-height': height + 'px'
                 })
                 .height(height);

             var $leftP = $('<p></p>')
                 .height(height)
                 .css({
                     'line-height': height + 'px',
                     'margin-left': 10 + 'px'
                 });
             var $rightP = $('<p></p>')
                 .height(height)
                 .css({
                     'line-height': height + 'px',
                     'margin-right': 10 + 'px'
                 });

             var $leftTextSpan = $('<span role="radio"></span>')
                 .addClass('slider-toggle-text')
                 .addClass('left')
                 .append($leftP)
                 .height(height);
             var $rightTextSpan = $('<span role="radio"></span>')
                 .addClass('slider-toggle-text')
                 .addClass('right')
                 .append($rightP)
                 .height(height);

             var $ballSpan = $('<span></span>')
                 .addClass('slider-toggle-ball')
                 .height(height - 1)
                 .width(ballWidth);

             var $frame = $('<div role="radiogroup"></div>')
                 .addClass('slider-toggle-frame')
                 .height(height)
                 .width(width)
                 .append($leftTextSpan)
                 .append($rightTextSpan)
                 .append($ballSpan)
                 .attr('aria-labelledby', $label.attr('id'))
                 .attr('id', $container.attr('id') + '_frame');

             // make the toggle container an ARIA application if it is not already part of one
             if ($container.parents('*[role=application]').length == 0) {
                 $container.attr('role', 'application');
             }

             $container
                 .height(height)
                 .append($frame);

             /*
              functions for toggling the slider left and right
              */

             var moveBallRight = function () {

                 $leftRadio.prop('checked', true);
                 $rightRadio.prop('checked', false);
                 $leftTextSpan.attr('aria-checked', 'true').attr('tabindex', tabIndex);
                 $rightTextSpan.attr('aria-checked', 'false').attr('tabindex', -1);
                 $leftP.text(onText);
                 $rightP.text(offText);
                 $leftTextSpan.removeClass('super');
                 $rightTextSpan.addClass('super');
                 $ballSpan.stop().animate({
                     left: ballRight
                 }, speed);
                 $leftTextSpan.stop().animate({
                     width: showWidth
                 }, speed);
                 $rightTextSpan.stop().animate({
                     width: hideWidth
                 }, {
                     duration: speed,
                     done: function () {
                         $rightP.text("");
                     }
                 });
             };

             var moveBallLeft = function () {
                 $leftRadio.prop('checked', false);
                 $rightRadio.prop('checked', true);
                 $leftTextSpan.attr('aria-checked', 'false').attr('tabindex', -1);
                 $rightTextSpan.attr('aria-checked', 'true').attr('tabindex', tabIndex);
                 $leftP.text(onText);
                 $rightP.text(offText);
                 $leftTextSpan.addClass('super');
                 $rightTextSpan.removeClass('super');
                 $ballSpan.stop().animate({
                     left: 0
                 }, speed);
                 $rightTextSpan.stop().animate({
                     width: showWidth
                 }, speed);
                 $leftTextSpan.stop().animate({
                     width: hideWidth
                 }, {
                     duration: speed,
                     done: function () {
                         $leftP.text("");
                     }
                 });
             };

             /*
              bind event handling to the slider components
              */

             $ballSpan.draggable({
                 containment: "parent",
                 scrollSpeed: speed,
                 drag: function (event, ui) {
                     $leftTextSpan.width(function () {
                         var w = ui.position.left + hideWidth;
                         if (w < width / 2) {
                             $leftTextSpan.removeClass('super');
                             $rightTextSpan.addClass('super');
                         } else {
                             $leftTextSpan.addClass('super');
                             $rightTextSpan.removeClass('super');
                         }
                         return w;
                     });
                     $rightTextSpan.width(function () {
                         return showWidth - ui.position.left;
                     });
                 },
                 start: function () {
                     $leftP.text(onText);
                     $rightP.text(offText);
                 },
                 stop: function (event, ui) {
                     if (ui.position.left < (width / 2) - (ballWidth / 2)) {
                         moveBallLeft();
                     } else {
                         moveBallRight();
                     }
                 }
             });

             $ballSpan.on(
                 "touchstart touchmove touchend touchcancel", function (event) {

                     if (event.originalEvent.touches.length > 1) {
                         return;
                     }

                     event.preventDefault();

                     var touch = event.originalEvent.changedTouches[0],
                         simulatedEvent = document.createEvent('MouseEvents');

                     var simulatedEvent = document.createEvent("MouseEvent");
                     simulatedEvent.initMouseEvent({
                         touchstart: "mousedown",
                         touchmove: "mousemove",
                         touchend: "mouseup"
                     }[event.type], true, true, window, 1,
                     touch.screenX, touch.screenY,
                     touch.clientX, touch.clientY, false,
                     false, false, false, 0, null);

                     event.target.dispatchEvent(simulatedEvent);

                 });

             $frame.focus(function () {
                 if ($leftRadio.is(':checked')) {
                     $leftTextSpan.focus();
                 } else {
                     $rightTextSpan.focus();
                 }
             });

             $frame.keydown(function (e) {

                 if (e.altKey) {
                     return true;
                 }

                 if (e.keyCode == keyCodes.left || e.keyCode == keyCodes.up || e.keyCode == keyCodes.right || e.keyCode == keyCodes.down) {

                     if (e.shiftKey) {
                         // do nothing
                         return true;
                     }
                     if (!$leftRadio.is(':checked')) {
                         moveBallRight();
                         $leftTextSpan.focus();
                     } else {
                         moveBallLeft();
                         $rightTextSpan.focus();
                     }

                     e.preventDefault();
                     e.stopPropagation();
                     return false;

                 } else if (e.keyCode == keyCodes.space || e.keyCode == keyCodes.enter) {
                     $frame.focus();
                     e.preventDefault();
                     e.stopPropagation();
                     return false;
                 }

                 return true;

             });

             $frame.click(function () {
                 if (!$leftRadio.is(':checked')) {
                     moveBallRight();
                 } else {
                     moveBallLeft();
                 }
             });


             /*
              initialize the state of the slider
              */

             var checked = $container.data('initialvalue') == $leftRadio.val();
             if (checked) {
                 $leftRadio.prop('checked', true);
                 $leftP.text(onText);
                 $rightP.text("");
                 $leftTextSpan.attr('aria-checked', 'true').attr('tabindex', tabIndex);
                 $leftTextSpan.width(showWidth);
                 $ballSpan.css({
                     left: ballRight
                 });
             } else {
                 $rightRadio.prop('checked', true);
                 $leftP.text("");
                 $rightP.text(offText);
                 $rightTextSpan.attr('aria-checked', 'true').attr('tabindex', tabIndex);
                 $rightTextSpan.width(showWidth);
                 $ballSpan.css({
                     left: 0
                 });
             }

         }

     };

     /**
      *  Creates a simple, user-friendly, 508-compliant, iPhone-style slider-toggles using a mix of HTML, CSS, and Javascript.
      *
      *  The plugin is designed to be used on hidden input elements.  For example, the following HTML:
      *   <pre>
      *      &lt;input id=&quot;mySliderToggle&quot; type=&quot;hidden&quot; name=&quot;myField&quot; value=&quot;0&quot;&gt;
      *   </pre>
      *   Can be turned into a slider-toggle with the following Javascript:
      *   <pre>
      *      $(#mySliderToggle).sliderToggle({
      *          height: 28,
      *          width: 78,
      *          ballwidth: 18,
      *          tabindex: 0,
      *          speed: 300
      *      });
      *   </pre>
      *
      *   Additionally, the generated toggle will inherit the title and tab index of the hidden input.
      *
      *   In order to work appropriately, pages uses this plugin must also link to the slider-toggle.less
      *
      *   <p/>
      *
      *   @author rees.byars
      */
     $.fn.sliderToggle = function (options) {

         return this.each(function () {

             if (!$.data(this, "plugin_" + sliderTogglePluginName)) {
                 $.data(this, "plugin_" + sliderTogglePluginName, new SliderToggle(this, options));
             }

         });

     };

     $.fn.sliderToggleVal = function () {

         var val = $(this).children(':checked').val();

         if (val === undefined) {
             val = $(this).data('initialvalue');
         }

         return val;

     };

 })(jQuery);

jQuery(document).ready(function () {

    jQuery('.slider-toggle-container').sliderToggle();

});