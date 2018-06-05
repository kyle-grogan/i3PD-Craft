angular.module('umbraco').directive('switch', function () {
    return {
        restrict: 'AE',
        replace: true,
        transclude: true,
        template: function (element, attrs) {
            var html = '';
            html += '<span';
            html += ' class="switcher' + (attrs.class ? ' ' + attrs.class : '') + '"';
            html += attrs.ngModel ? ' ng-click="' + attrs.ngModel + '=!' + attrs.ngModel + '"' : '';
            html += ' ng-class="{ checked:' + attrs.ngModel + ' }"';
            html += '>';
            html += '<small></small>';
            html += '<input type="checkbox"';
            html += attrs.id ? ' id="' + attrs.id + '-check"' : '';
            html += attrs.name ? ' name="' + attrs.name + '"' : '';
            html += attrs.ngModel ? ' ng-model="' + attrs.ngModel + '"' : '';
            html += ' style="display:none" />';
            html += '</span>';
            return html;
        }
    }
});

angular.module("umbraco").controller("USN.TrueFalse.Controller", function ($scope, $timeout, angularHelper) {

    var alreadyDirty = false;

    $scope.model.textRight = "";

    if ($scope.model.value === null || $scope.model.value === "") {
        $scope.enabled = 0;
    } else {
        $scope.enabled = $scope.model.value == 1;
    }

    $scope.$watch('enabled', function (newval, oldval) {

        $scope.model.value = newval === true ? 1 : 0;

        if ($scope.model.value == 1) {
            $scope.model.textRight = "Yes";

        }
        else {
            $scope.model.textRight = "No";
        }

        if (newval !== oldval) {
            //run after DOM is loaded
            $timeout(function () {
                if (!alreadyDirty) {
                    var currForm = angularHelper.getCurrentForm($scope);
                    currForm.$setDirty();
                    alreadyDirty = true;
                }
            }, 0);
        }

    }, true);

});