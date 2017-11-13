var Microsoft;
! function (Microsoft) {
    var Azure;
    ! function (Azure) {
        var IoT;
        ! function (IoT) {
            var App = function () {
                function App(notificationService, solutionsService, logger, settings) {
                    this.notificationService = notificationService, this.solutionsService = solutionsService, this.logger = logger, this.settings = settings, this.navigateTo = this.navigateTo.bind(this), this.onUnhandledError = this.onUnhandledError.bind(this), this.initialize = this.initialize.bind(this), this.onInitializationComplete = this.onInitializationComplete.bind(this), window.onerror = this.onUnhandledError, this.currentPage = ko.observable(), this.tenants = ko.observable(), this.selectedTenant = ko.observable(), this.isMooncake = ko.observable(IoT.Utils.isMooncake())
                }
                return App.prototype.initialize = function () {
                    var _this = this,
                        getSubscriptionPromise = this.solutionsService.getSubscriptions(),
                        getTenantsPromise = this.solutionsService.getTenants();
                    getSubscriptionPromise.done(function (subscriptions) {
                        0 === subscriptions.length ? _this.notificationService.dispatchError(IoT.ErrorMessages.azureSubscriptionRequired) : _this.settings.graph || _this.notificationService.dispatchError(IoT.ErrorMessages.accountHasNoPermissions)
                    }), getTenantsPromise.done(function (tenants) {
                        var selectedTenantId = _this.settings.tenant,
                            selectedTenant = tenants.first(function (x) {
                                return x.id === selectedTenantId
                            });
                        null == selectedTenant && (selectedTenant = tenants.first()), _this.selectedTenant(selectedTenant), _this.tenants(tenants)
                    })
                }, App.prototype.onInitializationComplete = function (subscriptions, tenants) {
                    0 === subscriptions.length ? this.notificationService.dispatchError(Resources.ErrorMessageAzureSubscriptionRequired) : this.settings.graph || this.notificationService.dispatchError(Resources.ErrorMessageInsufficientPermissions);
                    var selectedTenantId = this.settings.tenant,
                        selectedTenant = tenants.first(function (x) {
                            return x.id === selectedTenantId
                        });
                    null == selectedTenant && (selectedTenant = tenants.first()), this.selectedTenant(selectedTenant), this.tenants(tenants)
                }, App.prototype.onUnhandledError = function (errorMessage, url, lineNumber, columnNumber, errorObject) {
                    var errorContent;
                    errorContent = errorObject && void 0 !== errorObject.message ? errorObject.message : void 0 !== errorObject ? errorObject : errorMessage, this.logger.trackException(new Error(errorContent)), this.notificationService.dispatchError(Resources.ErrorMessageUnhandledException)
                }, App.prototype.navigateTo = function (pageName, requestParams) {
                    this.currentPage({
                        name: pageName,
                        params: requestParams
                    }), this.logger.trackPageView(pageName)
                }, App
            }();
            IoT.App = App
        }(IoT = Azure.IoT || (Azure.IoT = {}))
    }(Azure = Microsoft.Azure || (Microsoft.Azure = {}))
}(Microsoft || (Microsoft = {}));
var Microsoft;
! function (Microsoft) {
    var Azure;
    ! function (Azure) {
        var IoT;
        ! function (IoT) {
            var Constants = function () {
                function Constants() { }
                return Constants.refreshIntervalMs = 6e4, Constants.solutionNameRegExp = /^(?![0-9])(?!-)[a-zA-Z0-9-]{5,49}[a-zA-Z0-9]{1,1}$/, Constants
            }();
            IoT.Constants = Constants;
            var ErrorMessages = function () {
                function ErrorMessages() { }
                return ErrorMessages.couldNotLoadSolutions = Resources.ErrorMessageCouldNotLoadSolutions, ErrorMessages.couldNotDeleteSolution = Resources.ErrorMessageCouldNotDeleteSolution, ErrorMessages.azureSubscriptionRequired = Resources.ErrorMessageAzureSubscriptionRequired, ErrorMessages.couldNotLoadSubscriptions = Resources.ErrorMessageCouldNotLoadSubscriptions, ErrorMessages.couldNotInitProvisioning = Resources.ErrorMessageCouldNotInitProvisioning, ErrorMessages.reachedSolutionNumberLimit = Resources.ErrorMessageReachedSolutionNumberLimit, ErrorMessages.accountHasNoPermissions = Resources.ErrorMessageInsufficientPermissions, ErrorMessages.invalidSolutionName = Resources.ValidationMessageSolutionName, ErrorMessages
            }();
            IoT.ErrorMessages = ErrorMessages
        }(IoT = Azure.IoT || (Azure.IoT = {}))
    }(Azure = Microsoft.Azure || (Microsoft.Azure = {}))
}(Microsoft || (Microsoft = {})), String.prototype.contains = function (value, caseInsensitive) {
    void 0 === caseInsensitive && (caseInsensitive = !1);
    var originalValue = this;
    return caseInsensitive && (originalValue = originalValue.toLowerCase(), value = value.toLowerCase()), originalValue.indexOf(value) !== -1
}, String.prototype.format = function () {
    for (var values = [], _i = 0; _i < arguments.length; _i++) values[_i - 0] = arguments[_i];
    for (var formatted = this, i = 0; i < values.length; i++) {
        var regexp = new RegExp("\\{" + i + "\\}", "gi");
        formatted = values[i] ? formatted.replace(regexp, values[i]) : formatted.replace(regexp, "")
    }
    return formatted
}, Array.prototype.first = function (predicate) {
    var result = null;
    predicate || (predicate = function () {
        return !0
    });
    for (var i = 0; i < this.length; i++)
        if (predicate(this[i])) {
            result = this[i];
            break
        }
    return result
}, Array.prototype.remove = function (item) {
    var index = this.indexOf(item);
    this.splice(index, 1)
};
var Microsoft;
! function (Microsoft) {
    var Azure;
    ! function (Azure) {
        var IoT;
        ! function (IoT) {
            var injectableComponentLoader = {
                loadViewModel: function (name, config, callback) {
                    if (config.injectable) {
                        var viewModelConstructor = function (params) {
                            var resolvedInjectable = IoT.injector.resolve(config.injectable);
                            return resolvedInjectable.factory ? resolvedInjectable.factory(IoT.injector, params) : resolvedInjectable
                        };
                        ko.components.defaultLoader.loadViewModel(name, viewModelConstructor, callback)
                    } else callback(null)
                },
                loadTemplate: function (name, templateConfig, callback) {
                    if (templateConfig.fromUrl) {
                        var fullUrl = templateConfig.fromUrl;
                        $.get(fullUrl, function (markupString) {
                            ko.components.defaultLoader.loadTemplate(name, markupString, callback)
                        })
                    } else callback(null)
                }
            };
            ko.components.loaders.unshift(injectableComponentLoader)
        }(IoT = Azure.IoT || (Azure.IoT = {}))
    }(Azure = Microsoft.Azure || (Microsoft.Azure = {}))
}(Microsoft || (Microsoft = {}));
var Microsoft;
! function (Microsoft) {
    var Azure;
    ! function (Azure) {
        var IoT;
        ! function (IoT) {
            var Pages;
            ! function (Pages) {
                var Dashboard = function () {
                    function Dashboard(solutionsService, notificationService, logger) {
                        this.solutionsService = solutionsService, this.notificationService = notificationService, this.logger = logger, this.onSolutionsLoaded = this.onSolutionsLoaded.bind(this), this.onSolutionsLoadError = this.onSolutionsLoadError.bind(this), this.loadSolutions = this.loadSolutions.bind(this), this.refreshSolutions = this.refreshSolutions.bind(this), this.selectSolution = this.selectSolution.bind(this), this.closeSolutionDetails = this.closeSolutionDetails.bind(this), this.deleteSelectedSolution = this.deleteSelectedSolution.bind(this), this.solutions = ko.observableArray(), this.solutionsLoadingProgress = ko.observable(), this.solutionsRefreshProgress = ko.observable(), this.deletingSolutionProgress = ko.observable(), this.selectedSolution = ko.observable(), this.environmentSettings = ko.observable(), this.isMooncake = ko.observable(IoT.Utils.isMooncake()), this.loadSolutions(), notificationService.addSolutionUpdateListener(this.refreshSolutions), this.logger.trackEvent("Dashboard")
                    }
                    return Dashboard.prototype.loadSolutions = function () {
                        var _this = this;
                        this.solutionsLoadingProgress(!0), this.refreshSolutions().always(function () {
                            _this.solutionsLoadingProgress(!1)
                        })
                    }, Dashboard.prototype.refreshSolutions = function () {
                        this.solutionsRefreshProgress(!0), this.refreshTimeout && clearTimeout(this.refreshTimeout);
                        var getSolutionsPromise = this.solutionsService.getSolutions(),
                            getSolutionTypesPromise = this.solutionsService.getSolutionTypes();
                        return IoT.Promise.when(getSolutionsPromise, getSolutionTypesPromise).done(this.onSolutionsLoaded).fail(this.onSolutionsLoadError)
                    }, Dashboard.prototype.onSolutionsLoaded = function (solutionContracts, solutionTypes) {
                        var _this = this;
                        solutionContracts.forEach(function (solutionContract) {
                            var existingSolutionObservable = _this.solutions().first(function (x) {
                                return x().name === solutionContract.name
                            }),
                                solution = new IoT.Solution,
                                solutionType = solutionTypes.first(function (x) {
                                    return x.id === solutionContract.typeId
                                });
                            solution.type = solutionType, solution.name = solutionContract.name, solution.region = solutionContract.region, solution.resourceGroupUrl = solutionContract.resourceGroupUrl, solution.state = solutionContract.state, solution.subscriptionId = solutionContract.subscriptionId, solution.dashboardUrl = solutionContract.dashboardUrl, solution.links = solutionContract.links, solution.createTime = solutionContract.createTime, solution.updateTime = solutionContract.updateTime, solution.resources = solutionContract.resources, solution.logs = solutionContract.logs, solution.description = solutionContract.description || solutionType.shortDescription, existingSolutionObservable ? moment(solutionContract.updateTime) > moment(existingSolutionObservable().updateTime) && existingSolutionObservable(solution) : _this.solutions.push(ko.observable(solution))
                        }), this.solutions().forEach(function (existingObservable) {
                            var existingSolution = existingObservable();
                            existingSolution.state !== IoT.SolutionState.provisioning && 0 === solutionContracts.filter(function (y) {
                                return y.name === existingSolution.name
                            }).length && _this.solutions.remove(existingObservable)
                        });
                        var selectedSolution = this.selectedSolution();
                        if (selectedSolution) {
                            var updateSolution = this.solutions().first(function (x) {
                                return x().name === selectedSolution.name
                            });
                            updateSolution ? this.selectedSolution(updateSolution()) : this.closeSolutionDetails()
                        }
                        this.refreshTimeout = setTimeout(this.refreshSolutions, IoT.Constants.refreshIntervalMs), this.solutionsRefreshProgress(!1)
                    }, Dashboard.prototype.onSolutionsLoadError = function () {
                        this.solutionsLoadingProgress(!1), this.notificationService.dispatchError(IoT.ErrorMessages.couldNotLoadSolutions)
                    }, Dashboard.prototype.loadEnvironmentSettings = function (region) {
                        var _this = this;
                        if (null == this.environmentSettings()) {
                            var solutionEnvironmentSettingsPromise = this.solutionsService.getEnvironmentSettings(region);
                            solutionEnvironmentSettingsPromise.done(function (settings) {
                                _this.environmentSettings(settings)
                            })
                        }
                    }, Dashboard.prototype.selectSolution = function (solution) {
                        return this.selectedSolution() === solution ? this.closeSolutionDetails() : (this.selectedSolution(solution), this.loadEnvironmentSettings(solution.region)), !0
                    }, Dashboard.prototype.closeSolutionDetails = function () {
                        this.selectedSolution(null)
                    }, Dashboard.prototype.deleteSelectedSolution = function () {
                        var _this = this,
                            solution = this.selectedSolution(),
                            deleteSolutionPromise = this.solutionsService.deleteSolution(solution.subscriptionId, solution.name);
                        this.deletingSolutionProgress(!0), deleteSolutionPromise.done(function () {
                            var refreshSolutionsPromise = _this.refreshSolutions();
                            refreshSolutionsPromise.done(function () {
                                _this.closeSolutionDetails(), _this.deletingSolutionProgress(!1), $("#deleteSolutionModal").modal("hide")
                            })
                        }), deleteSolutionPromise.fail(function () {
                            _this.notificationService.dispatchError(IoT.ErrorMessages.couldNotDeleteSolution)
                        })
                    }, Dashboard
                }();
                Pages.Dashboard = Dashboard
            }(Pages = IoT.Pages || (IoT.Pages = {}))
        }(IoT = Azure.IoT || (Azure.IoT = {}))
    }(Azure = Microsoft.Azure || (Microsoft.Azure = {}))
}(Microsoft || (Microsoft = {}));
var Microsoft;
! function (Microsoft) {
    var Azure;
    ! function (Azure) {
        var IoT;
        ! function (IoT) {
            var Pages;
            ! function (Pages) {
                var CreateSolution = function () {
                    function CreateSolution(solutionsService, notificationService, logger, router, solutionTypeId) {
                        var _this = this;
                        this.solutionsService = solutionsService, this.notificationService = notificationService, this.logger = logger, this.router = router, this.provisionSolution = this.provisionSolution.bind(this), this.onRegionsLoaded = this.onRegionsLoaded.bind(this), this.onSubscriptionsLoaded = this.onSubscriptionsLoaded.bind(this), this.validateSolutionName = this.validateSolutionName.bind(this), this.scheduleSolutionNameValidation = this.scheduleSolutionNameValidation.bind(this), this.validateSubscription = this.validateSubscription.bind(this), this.readyToProvision = this.readyToProvision.bind(this), this.onSolutionTypeLoaded = this.onSolutionTypeLoaded.bind(this), this.loadPageData = this.loadPageData.bind(this), this.onPageDataLoaded = this.onPageDataLoaded.bind(this), this.onProvisionRequestFailed = this.onProvisionRequestFailed.bind(this), this.onProvisionRequestSent = this.onProvisionRequestSent.bind(this), this.onStaticMapChecked = this.onStaticMapChecked.bind(this), this.regions = ko.observableArray(), this.subscriptions = ko.observableArray(), this.selectedRegion = ko.observable(), this.selectedSubscription = ko.observable(), this.validationMessage = ko.observable(), this.nameIsValid = ko.observable(!1), this.solutionName = ko.observable(), this.loadInProgress = ko.observable(!0), this.solutionNameValidationProgress = ko.observable(!1), this.provisioningInProgress = ko.observable(!1), this.solutionTypeName = ko.observable(), this.solutionPrice = ko.observable(), this.staticMapAllowed = ko.observable(!1), this.validSubscription = ko.observable(!1), this.subscriptionValidationInProgress = ko.observable(!1), this.resourceErrorMessage = ko.observable(), this.mapsErrorMessage = ko.observable(), this.isMooncake = ko.observable(IoT.Utils.isMooncake()), this.deploymentOptions = [{
                            text: Resources.LabelBasic,
                            value: "Basic"
                        }, {
                            text: Resources.LabelStandard,
                            value: "Standard"
                        }], this.languageOptions = [{
                            text: ".NET",
                            value: "dotnet"
                        }, {
                            text: "Java",
                            value: "java"
                        }], this.deploymentType = ko.observable(this.deploymentOptions[0].value), this.languageType = ko.observable(this.languageOptions[0].value), this.isBasicDeployment = ko.computed({
                            owner: this,
                            read: function () {
                                return _this.deploymentType() === _this.deploymentOptions[0].value
                            }
                        }), this.solutionName.subscribe(this.scheduleSolutionNameValidation), this.selectedSubscription.subscribe(this.validateSubscription), this.staticMapAllowed.subscribe(this.onStaticMapChecked), this.loadPageData(solutionTypeId), this.logger.trackEvent("Create solution"), this.initializeToolTips()
                    }
                    return CreateSolution.prototype.initializeToolTips = function () {
                        for (var tooltips = Array.prototype.slice.apply($('[data-toggle="tooltip"]')), _i = 0, tooltips_1 = tooltips; _i < tooltips_1.length; _i++) {
                            var t = tooltips_1[_i];
                            $(t).tooltip()
                        }
                    }, CreateSolution.prototype.loadPageData = function (solutionTypeId) {
                        this.loadInProgress(!0);
                        var getRegionsPromise = this.solutionsService.getRegions();
                        getRegionsPromise.done(this.onRegionsLoaded);
                        var getSubscriptionsPromise = this.solutionsService.getSubscriptions();
                        getSubscriptionsPromise.done(this.onSubscriptionsLoaded);
                        var getSolutionTypePromise = this.solutionsService.getSolutionType(solutionTypeId);
                        getSolutionTypePromise.done(this.onSolutionTypeLoaded);
                        var loadPageDataPromise = IoT.Promise.when(getRegionsPromise, getSubscriptionsPromise, getSolutionTypePromise);
                        loadPageDataPromise.done(this.onPageDataLoaded)
                    }, CreateSolution.prototype.onPageDataLoaded = function () {
                        this.loadInProgress(!1)
                    }, CreateSolution.prototype.onSolutionTypeLoaded = function (solutionType) {
                        this.solutionType = solutionType, this.solutionTypeName(solutionType.name), this.solutionPrice(solutionType.price)
                    }, CreateSolution.prototype.onRegionsLoaded = function (regions) {
                        this.regions(regions)
                    }, CreateSolution.prototype.onSubscriptionsLoaded = function (subscriptions) {
                        this.subscriptions(subscriptions)
                    }, CreateSolution.prototype.scheduleSolutionNameValidation = function () {
                        clearTimeout(this.solutionNameTimeoutHandle), this.validationMessage(""), this.solutionNameValidationProgress(!1), this.nameIsValid(!1), 0 !== this.solutionName().length && (this.validationTag = Math.floor(Math.random() * Number.MAX_VALUE).toString(36), this.solutionNameTimeoutHandle = setTimeout(this.validateSolutionName, 500, this.validationTag))
                    }, CreateSolution.prototype.validateSolutionName = function (tag) {
                        var _this = this,
                            solutionName = this.solutionName(),
                            isValid = IoT.Constants.solutionNameRegExp.test(solutionName);
                        if (!isValid) return void this.validationMessage(IoT.ErrorMessages.invalidSolutionName);
                        this.solutionNameValidationProgress(!0);
                        var validateSolutionNamePromise = this.solutionsService.validateSolutionName(this.subscriptions()[0].id, solutionName);
                        validateSolutionNamePromise.done(function (result) {
                            if (_this.validationTag === tag && _this.solutionNameValidationProgress()) return _this.solutionNameValidationProgress(!1), result.isValid ? void _this.nameIsValid(!0) : void _this.validationMessage(result.message)
                        })
                    }, CreateSolution.prototype.validateSubscription = function () {
                        var _this = this;
                        return this.validSubscription(!1), this.subscriptionValidationInProgress(!0), this.staticMapAllowed(!1), this.mapsErrorMessage(null), this.resourceErrorMessage(null), this.selectedSubscription() ? (console.log(this.solutionType), this.solutionsService.validateSubscription(this.selectedSubscription().id, this.solutionType.id).then(function (validationResult) {
                            _this.subscriptionValidationInProgress(!1), _this.resourceErrorMessage(validationResult.resourceErrorMessage), _this.mapsErrorMessage(validationResult.bingMapsErrorMessage), _this.isMooncake() && _this.mapsErrorMessage() && _this.mapsErrorMessage(_this.mapsErrorMessage().replace("azure.microsoft.com/documentation/articles/iot-suite-faq", "www.azure.cn/documentation/articles/iot-suite-faq"));
                            var isValid = !_this.resourceErrorMessage() && !_this.mapsErrorMessage();
                            _this.validSubscription(isValid)
                        }), void this.solutionsService.getAvailableRegions(this.selectedSubscription().id, this.solutionType.id).then(function (availableRegions) {
                            _this.regions(availableRegions)
                        })) : void this.subscriptionValidationInProgress(!1)
                    }, CreateSolution.prototype.onStaticMapChecked = function () {
                        this.staticMapAllowed() ? this.validSubscription(!0) : this.validSubscription(!1)
                    }, CreateSolution.prototype.onProvisionRequestSent = function (result) {
                        return result.isSuccessful ? void this.router.navigateToHome() : (this.provisioningInProgress(!1), void this.notificationService.dispatchError(result.message))
                    }, CreateSolution.prototype.onProvisionRequestFailed = function () {
                        this.provisioningInProgress(!1), this.notificationService.dispatchError(IoT.ErrorMessages.couldNotInitProvisioning)
                    }, CreateSolution.prototype.readyToProvision = function () {
                        return this.nameIsValid() && null != this.selectedRegion() && this.validSubscription() && !this.provisioningInProgress()
                    }, CreateSolution.prototype.isRM = function () {
                        return IoT.SolutionTypes.RemoteMonitoring == this.solutionType.id
                    }, CreateSolution.prototype.isPCS2 = function () {
                        return IoT.SolutionTypes.RemoteMonitoringV2 == this.solutionType.id
                    }, CreateSolution.prototype.provisionSolution = function () {
                        if (this.readyToProvision()) {
                            var solutionType, solutionName = this.solutionName(),
                                regionId = this.selectedRegion().id,
                                subscriptionId = this.selectedSubscription().id;
                            solutionType = this.isRM() && this.staticMapAllowed() ? IoT.SolutionTypes.RemoteMonitoringStatic : this.solutionType.id;
                            var additionalOptions = {};
                            this.solutionType.additionalOptions ? this.solutionType.additionalOptions.forEach(function (option) {
                                additionalOptions[option.name] = option.value
                            }) : additionalOptions["default"] = "", this.staticMapAllowed() && (additionalOptions.StaticMap = "true"), this.isPCS2() && (additionalOptions.LanguageType = this.languageType());
                            var provisionSolutionPromise = this.solutionsService.provisionSolution(solutionName, solutionType, subscriptionId, regionId, additionalOptions);
                            this.provisioningInProgress(!0), provisionSolutionPromise.done(this.onProvisionRequestSent), provisionSolutionPromise.fail(this.onProvisionRequestFailed)
                        }
                    }, CreateSolution.prototype.normalizeTooltipText = function (text) {
                        return text ? text.replace(/<(.*?)\/?>/g, "") : null
                    }, CreateSolution
                }();
                Pages.CreateSolution = CreateSolution
            }(Pages = IoT.Pages || (IoT.Pages = {}))
        }(IoT = Azure.IoT || (Azure.IoT = {}))
    }(Azure = Microsoft.Azure || (Microsoft.Azure = {}))
}(Microsoft || (Microsoft = {}));
var Microsoft;
! function (Microsoft) {
    var Azure;
    ! function (Azure) {
        var IoT;
        ! function (IoT) {
            var Pages;
            ! function (Pages) {
                var SelectSolution = function () {
                    function SelectSolution(solutionsService, notificationService, logger, settings) {
                        this.solutionsService = solutionsService, this.notificationService = notificationService, this.logger = logger, this.settings = settings, this.onRequestError = this.onRequestError.bind(this), this.loadPageData = this.loadPageData.bind(this), this.onPageDataLoaded = this.onPageDataLoaded.bind(this), this.solutionTypes = ko.observableArray(), this.requestInProgress = ko.observable(!1), this.readyToProvision = ko.observable(!1), this.loadPageData(), this.logger.trackEvent("Select solution")
                    }
                    return SelectSolution.prototype.onRequestError = function () {
                        this.requestInProgress(!1), this.notificationService.dispatchError(IoT.ErrorMessages.couldNotLoadSubscriptions)
                    }, SelectSolution.prototype.onPageDataLoaded = function (solutionTypes, subscriptions) {
                        this.requestInProgress(!1), this.solutionTypes(solutionTypes), subscriptions.length > 0 && this.settings.graph && this.readyToProvision(!0)
                    }, SelectSolution.prototype.loadPageData = function () {
                        this.requestInProgress(!0);
                        var getSolutionTypesPromise = this.solutionsService.getSolutionTypes(),
                            getSubscriptionsPromise = this.solutionsService.getSubscriptions(),
                            loadPageDataPromise = IoT.Promise.when(getSolutionTypesPromise, getSubscriptionsPromise);
                        loadPageDataPromise.done(this.onPageDataLoaded), loadPageDataPromise.fail(this.onRequestError)
                    }, SelectSolution
                }();
                Pages.SelectSolution = SelectSolution
            }(Pages = IoT.Pages || (IoT.Pages = {}))
        }(IoT = Azure.IoT || (Azure.IoT = {}))
    }(Azure = Microsoft.Azure || (Microsoft.Azure = {}))
}(Microsoft || (Microsoft = {}));
var Microsoft;
! function (Microsoft) {
    var Azure;
    ! function (Azure) {
        var IoT;
        ! function (IoT) {
            var Components;
            ! function (Components) {
                var Messages = function () {
                    function Messages(notificationService) {
                        this.notificationService = notificationService, this.addErrorMessage = this.addErrorMessage.bind(this), this.scheduleMessageClosing = this.scheduleMessageClosing.bind(this), this.closeMessage = this.closeMessage.bind(this), this.messages = ko.observableArray(), this.notificationService.addErrorListener(this.addErrorMessage)
                    }
                    return Messages.prototype.scheduleMessageClosing = function (message) {
                        setTimeout(function () { }, 6e3)
                    }, Messages.prototype.closeMessage = function (message) {
                        this.messages.remove(message)
                    }, Messages.prototype.addErrorMessage = function (messageContent) {
                        var message = new IoT.Message;
                        message.content = messageContent, message.type = "error", message.close = this.closeMessage, this.messages.push(message), this.scheduleMessageClosing(message)
                    }, Messages
                }();
                Components.Messages = Messages
            }(Components = IoT.Components || (IoT.Components = {}))
        }(IoT = Azure.IoT || (Azure.IoT = {}))
    }(Azure = Microsoft.Azure || (Microsoft.Azure = {}))
}(Microsoft || (Microsoft = {}));
var Microsoft;
! function (Microsoft) {
    var Azure;
    ! function (Azure) {
        var IoT;
        ! function (IoT) {
            ko.components.register("messages", {
                template: {
                    fromUrl: "content/components/messages/messages.html"
                },
                viewModel: {
                    injectable: "messages"
                }
            }), ko.components.register("dashboard", {
                template: {
                    fromUrl: "content/pages/dashboard/dashboard.html"
                },
                viewModel: {
                    injectable: "dashboard"
                }
            }), ko.components.register("selectSolution", {
                template: {
                    fromUrl: "content/pages/selectSolution/selectSolution.html"
                },
                viewModel: {
                    injectable: "selectSolution"
                }
            }), ko.components.register("createSolution", {
                template: {
                    fromUrl: "content/pages/createSolution/createSolution.html"
                },
                viewModel: {
                    injectable: "createSolution"
                }
            })
        }(IoT = Azure.IoT || (Azure.IoT = {}))
    }(Azure = Microsoft.Azure || (Microsoft.Azure = {}))
}(Microsoft || (Microsoft = {}));
var Microsoft;
! function (Microsoft) {
    var Azure;
    ! function (Azure) {
        var IoT;
        ! function (IoT) {
            var Promise = function () {
                function Promise() { }
                return Promise.when = function () {
                    for (var promises = [], _i = 0; _i < arguments.length; _i++) promises[_i - 0] = arguments[_i];
                    var deferred = $.Deferred(),
                        completedCount = 0,
                        results = [];
                    return promises.forEach(function (task, index) {
                        task.done(function (result) {
                            results[index] = result, completedCount++ , completedCount === promises.length && deferred.resolve.apply(null, results)
                        }), task.fail(function () {
                            deferred.reject()
                        })
                    }), deferred.promise()
                }, Promise.fromResult = function (result) {
                    var deferred = $.Deferred();
                    return deferred.resolve(result), deferred.promise()
                }, Promise
            }();
            IoT.Promise = Promise
        }(IoT = Azure.IoT || (Azure.IoT = {}))
    }(Azure = Microsoft.Azure || (Microsoft.Azure = {}))
}(Microsoft || (Microsoft = {}));
var Microsoft;
! function (Microsoft) {
    var Azure;
    ! function (Azure) {
        var IoT;
        ! function (IoT) {
            var InversifyInjector = function () {
                function InversifyInjector() {
                    this.kernel = new inversify.Kernel, this.bindSingleton = this.bindSingleton.bind(this), this.bind = this.bind.bind(this), this.bindFactory = this.bindFactory.bind(this)
                }
                return InversifyInjector.prototype.bind = function (name, transient) {
                    this.kernel.bind(new inversify.TypeBinding(name, transient, inversify.TypeBindingScopeEnum.Transient))
                }, InversifyInjector.prototype.bindSingleton = function (name, singletone) {
                    this.kernel.bind(new inversify.TypeBinding(name, singletone, inversify.TypeBindingScopeEnum.Singleton))
                }, InversifyInjector.prototype.bindFactory = function (name, factory) {
                    var construct = function () {
                        this.factory = factory
                    };
                    this.kernel.bind(new inversify.TypeBinding(name, construct, inversify.TypeBindingScopeEnum.Singleton))
                }, InversifyInjector.prototype.bindInstance = function (name, instance) {
                    var construct = function () {
                        return instance
                    };
                    this.kernel.bind(new inversify.TypeBinding(name, construct, inversify.TypeBindingScopeEnum.Singleton))
                }, InversifyInjector.prototype.resolve = function (runtimeIdentifier) {
                    return this.kernel.resolve(runtimeIdentifier)
                }, InversifyInjector
            }();
            IoT.InversifyInjector = InversifyInjector
        }(IoT = Azure.IoT || (Azure.IoT = {}))
    }(Azure = Microsoft.Azure || (Microsoft.Azure = {}))
}(Microsoft || (Microsoft = {}));
var Microsoft;
! function (Microsoft) {
    var Azure;
    ! function (Azure) {
        var IoT;
        ! function (IoT) {
            var CrossroadsRouter = function () {
                function CrossroadsRouter() {
                    this.parseHash = this.parseHash.bind(this)
                }
                return CrossroadsRouter.prototype.parseHash = function (newHash, oldHash) {
                    crossroads.parse(newHash)
                }, CrossroadsRouter.prototype.addRouteHandler = function (pattern, handler, priority) {
                    crossroads.addRoute(pattern, handler, priority)
                }, CrossroadsRouter.prototype.startListening = function () {
                    crossroads.normalizeFn = crossroads.NORM_AS_OBJECT, hasher.initialized.add(this.parseHash), hasher.changed.add(this.parseHash), hasher.init()
                }, CrossroadsRouter.prototype.navigateTo = function (route) {
                    hasher.setHash(route)
                }, CrossroadsRouter.prototype.navigateToHome = function () {
                    hasher.setHash("")
                }, CrossroadsRouter
            }();
            IoT.CrossroadsRouter = CrossroadsRouter
        }(IoT = Azure.IoT || (Azure.IoT = {}))
    }(Azure = Microsoft.Azure || (Microsoft.Azure = {}))
}(Microsoft || (Microsoft = {}));
var Microsoft;
! function (Microsoft) {
    var Azure;
    ! function (Azure) {
        var IoT;
        ! function (IoT) {
            var JQueryHttpClient = function () {
                function JQueryHttpClient() {
                    this.get = this.get.bind(this), this.post = this.post.bind(this), this.patch = this.patch.bind(this), this.put = this.put.bind(this), this["delete"] = this["delete"].bind(this), this.head = this.head.bind(this), this.onBeforeSend = this.onBeforeSend.bind(this), this.handleErrorResponse = this.handleErrorResponse.bind(this), this.handle401Response = this.handle401Response.bind(this), this.urlFormat = "/{0}"
                }
                return JQueryHttpClient.prototype.onBeforeSend = function (xhrObj) {
                    xhrObj.setRequestHeader("X-Correlation-ID", IoT.Utils.correlationId)
                }, JQueryHttpClient.prototype.handleErrorResponse = function (xmlHttpRequest) {
                    switch (xmlHttpRequest.status) {
                        case 401:
                            this.handle401Response(xmlHttpRequest)
                    }
                }, JQueryHttpClient.prototype.handle401Response = function (xmlHttpRequest) {
                    window.location.reload()
                }, JQueryHttpClient.prototype.get = function (url) {
                    var promise = jQuery.ajax({
                        url: this.urlFormat.format(url),
                        method: "GET",
                        beforeSend: this.onBeforeSend,
                        error: this.handleErrorResponse,
                        cache: !1
                    });
                    return promise
                }, JQueryHttpClient.prototype.post = function (url, payload) {
                    var promise = jQuery.ajax({
                        url: this.urlFormat.format(url),
                        method: "POST",
                        data: payload,
                        beforeSend: this.onBeforeSend,
                        error: this.handleErrorResponse,
                        cache: !1
                    });
                    return promise
                }, JQueryHttpClient.prototype.patch = function (url, payload) {
                    var promise = jQuery.ajax({
                        url: this.urlFormat.format(url),
                        method: "PATCH",
                        data: payload,
                        beforeSend: this.onBeforeSend,
                        error: this.handleErrorResponse,
                        cache: !1
                    });
                    return promise
                }, JQueryHttpClient.prototype.put = function (url, payload) {
                    var promise = jQuery.ajax({
                        url: this.urlFormat.format(url),
                        method: "PUT",
                        data: payload,
                        beforeSend: this.onBeforeSend,
                        error: this.handleErrorResponse,
                        cache: !1
                    });
                    return promise
                }, JQueryHttpClient.prototype["delete"] = function (url) {
                    var promise = jQuery.ajax({
                        url: this.urlFormat.format(url),
                        method: "DELETE",
                        beforeSend: this.onBeforeSend,
                        error: this.handleErrorResponse,
                        cache: !1
                    });
                    return promise
                }, JQueryHttpClient.prototype.head = function (url) {
                    var promise = jQuery.ajax({
                        url: this.urlFormat.format(url),
                        method: "HEAD",
                        beforeSend: this.onBeforeSend,
                        error: this.handleErrorResponse,
                        cache: !1
                    });
                    return promise
                }, JQueryHttpClient
            }();
            IoT.JQueryHttpClient = JQueryHttpClient
        }(IoT = Azure.IoT || (Azure.IoT = {}))
    }(Azure = Microsoft.Azure || (Microsoft.Azure = {}))
}(Microsoft || (Microsoft = {}));
var Microsoft;
! function (Microsoft) {
    var Azure;
    ! function (Azure) {
        var IoT;
        ! function (IoT) {
            var JQueryPromiseHelper = function () {
                function JQueryPromiseHelper() { }
                return JQueryPromiseHelper.fromResult = function (result) {
                    var deffered = $.Deferred();
                    return deffered.resolve(result), deffered.promise()
                }, JQueryPromiseHelper
            }();
            IoT.JQueryPromiseHelper = JQueryPromiseHelper
        }(IoT = Azure.IoT || (Azure.IoT = {}))
    }(Azure = Microsoft.Azure || (Microsoft.Azure = {}))
}(Microsoft || (Microsoft = {}));
var Microsoft;
! function (Microsoft) {
    var Azure;
    ! function (Azure) {
        var IoT;
        ! function (IoT) {
            var SolutionState = function () {
                function SolutionState() { }
                return SolutionState.ready = "Ready", SolutionState.provisioning = "Provisioning", SolutionState.deleting = "Deleting", SolutionState.failed = "Failed", SolutionState
            }();
            IoT.SolutionState = SolutionState
        }(IoT = Azure.IoT || (Azure.IoT = {}))
    }(Azure = Microsoft.Azure || (Microsoft.Azure = {}))
}(Microsoft || (Microsoft = {}));
var Microsoft;
! function (Microsoft) {
    var Azure;
    ! function (Azure) {
        var IoT;
        ! function (IoT) {
            var SolutionsService = function () {
                function SolutionsService(httpClient) {
                    this.httpClient = httpClient, this.getSubscriptions = this.getSubscriptions.bind(this), this.getRegions = this.getRegions.bind(this), this.getSolutions = this.getSolutions.bind(this), this.getSolutionTypes = this.getSolutionTypes.bind(this), this.deleteSolution = this.deleteSolution.bind(this), this.getEnvironmentSettings = this.getEnvironmentSettings.bind(this), this.solutions = new Array
                }
                return SolutionsService.prototype.getTenants = function () {
                    return this.httpClient.get("tenants")
                }, SolutionsService.prototype.getSubscriptions = function () {
                    return this.httpClient.get("subscriptions")
                }, SolutionsService.prototype.getRegions = function () {
                    return this.httpClient.get("regions")
                }, SolutionsService.prototype.getSolutions = function () {
                    return this.httpClient.get("solutions/provisioned")
                }, SolutionsService.prototype.getSolutionTypes = function () {
                    var deffered = $.Deferred();
                    if (this.solutionTypes) deffered.resolve(this.solutionTypes);
                    else {
                        var promise = this.httpClient.get("solutions/types");
                        promise.done(function (solutionTypes) {
                            deffered.resolve(solutionTypes)
                        }), promise.fail(function () {
                            deffered.reject()
                        })
                    }
                    return deffered.promise()
                }, SolutionsService.prototype.getSolutionType = function (solutionTypeId) {
                    return this.httpClient.get("solutions/types/{0}".format(solutionTypeId))
                }, SolutionsService.prototype.deleteSolution = function (subscriptionId, solutionName) {
                    return this.httpClient["delete"]("subscriptions/{0}/solutions/{1}".format(subscriptionId, solutionName))
                }, SolutionsService.prototype.provisionSolution = function (solutionName, solutionType, subscriptionId, regionName, additionalOptions) {
                    return this.httpClient.put("solutions/provisioned", {
                        name: solutionName,
                        type: solutionType,
                        subscriptionId: subscriptionId,
                        region: regionName,
                        additionalOptions: additionalOptions
                    })
                }, SolutionsService.prototype.validateSolutionName = function (subscriptionId, solutionName) {
                    return this.httpClient.post("solutions/name", {
                        subscriptionId: subscriptionId,
                        solutionName: solutionName
                    })
                }, SolutionsService.prototype.validateSubscription = function (subscriptionId, solutionType) {
                    return this.httpClient.get("subscriptions/" + subscriptionId + "/types/" + solutionType)
                }, SolutionsService.prototype.getSolution = function (subscriptionId, solutionName) {
                    return this.httpClient.get("subscriptions/{0}/solutions/{1}".format(subscriptionId, solutionName))
                }, SolutionsService.prototype.getEnvironmentSettings = function (location) {
                    return this.httpClient.get("environments/{0}".format(location))
                }, SolutionsService.prototype.getAvailableRegions = function (subscriptionId, solutionType) {
                    return this.httpClient.get("subscriptions/{0}/types/{1}/regions".format(subscriptionId, solutionType))
                }, SolutionsService
            }();
            IoT.SolutionsService = SolutionsService
        }(IoT = Azure.IoT || (Azure.IoT = {}))
    }(Azure = Microsoft.Azure || (Microsoft.Azure = {}))
}(Microsoft || (Microsoft = {}));
var Microsoft;
! function (Microsoft) {
    var Azure;
    ! function (Azure) {
        var IoT;
        ! function (IoT) {
            var AppInsightsLogger = function () {
                function AppInsightsLogger(instrumentationKey) {
                    this.trackException = this.trackException.bind(this), this.trackPageView = this.trackPageView.bind(this);
                    var configuration = {
                        instrumentationKey: instrumentationKey
                    };
                    this.appInsightsClient = new Microsoft.ApplicationInsights.AppInsights(configuration)
                }
                return AppInsightsLogger.prototype.trackPageView = function (name) {
                    this.appInsightsClient.trackPageView(name)
                }, AppInsightsLogger.prototype.trackException = function (exception) {
                    this.appInsightsClient.trackException(exception)
                }, AppInsightsLogger.prototype.trackEvent = function (name, properties) {
                    this.appInsightsClient.trackEvent(name, properties)
                }, AppInsightsLogger
            }();
            IoT.AppInsightsLogger = AppInsightsLogger
        }(IoT = Azure.IoT || (Azure.IoT = {}))
    }(Azure = Microsoft.Azure || (Microsoft.Azure = {}))
}(Microsoft || (Microsoft = {}));
var Microsoft;
! function (Microsoft) {
    var Azure;
    ! function (Azure) {
        var IoT;
        ! function (IoT) {
            var SessionStorageCache = function () {
                function SessionStorageCache() { }
                return SessionStorageCache.prototype.setItem = function (key, value) {
                    window.sessionStorage.setItem(key, JSON.stringify(value))
                }, SessionStorageCache.prototype.getItem = function (key) {
                    var value = window.sessionStorage.getItem(key);
                    return value ? JSON.parse(value) : null
                }, SessionStorageCache.prototype.removeItem = function (key) {
                    window.sessionStorage.removeItem(key)
                }, SessionStorageCache.prototype.clear = function () {
                    window.sessionStorage.clear()
                }, SessionStorageCache
            }();
            IoT.SessionStorageCache = SessionStorageCache
        }(IoT = Azure.IoT || (Azure.IoT = {}))
    }(Azure = Microsoft.Azure || (Microsoft.Azure = {}))
}(Microsoft || (Microsoft = {}));
var Microsoft;
! function (Microsoft) {
    var Azure;
    ! function (Azure) {
        var IoT;
        ! function (IoT) {
            var NotificationService = function () {
                function NotificationService() {
                    this.dispatchError = this.dispatchError.bind(this), this.addErrorListener = this.addErrorListener.bind(this), this.onNotification = new ko.subscribable;
                    var connection = $.hubConnection();
                    this.hubProxy = connection.createHubProxy("hub"), connection.start()
                }
                return NotificationService.prototype.dispatchError = function (messageContent) {
                    this.onNotification.notifySubscribers(messageContent)
                }, NotificationService.prototype.addErrorListener = function (callback) {
                    this.onNotification.subscribe(callback)
                }, NotificationService.prototype.addSolutionUpdateListener = function (callback) {
                    this.hubProxy.on("solutionUpdate", callback)
                }, NotificationService.prototype.addSolutionResourceUpdateListener = function (callback) {
                    this.hubProxy.on("solutionResourceUpdate", callback)
                }, NotificationService.prototype.addRefreshRequestListener = function (callback) {
                    this.hubProxy.on("refreshRequest", callback)
                }, NotificationService
            }();
            IoT.NotificationService = NotificationService
        }(IoT = Azure.IoT || (Azure.IoT = {}))
    }(Azure = Microsoft.Azure || (Microsoft.Azure = {}))
}(Microsoft || (Microsoft = {}));
var Microsoft;
! function (Microsoft) {
    var Azure;
    ! function (Azure) {
        var IoT;
        ! function (IoT) {
            function bootstrap(settings) {
                moment.locale(document.documentElement.lang);
                var router = new IoT.CrossroadsRouter;
                IoT.injector.bindSingleton("app", IoT.App), IoT.injector.bindInstance("settings", settings), IoT.injector.bindInstance("router", router), IoT.injector.bind("messages", IoT.Components.Messages), IoT.injector.bind("dashboard", IoT.Pages.Dashboard), IoT.injector.bind("selectSolution", IoT.Pages.SelectSolution), IoT.injector.bindFactory("createSolution", function (ctx, routeParams) {
                    var solutionsService = ctx.resolve("solutionsService"),
                        notificationService = ctx.resolve("notificationService"),
                        logger = ctx.resolve("logger"),
                        router = ctx.resolve("router");
                    return new IoT.Pages.CreateSolution(solutionsService, notificationService, logger, router, routeParams.solutionTypeId)
                }), IoT.injector.bindSingleton("notificationService", IoT.NotificationService), IoT.injector.bindSingleton("solutionsService", IoT.SolutionsService), IoT.injector.bindSingleton("httpClient", IoT.JQueryHttpClient), IoT.injector.bindSingleton("localCache", IoT.SessionStorageCache), IoT.injector.bindInstance("logger", new IoT.AppInsightsLogger(settings.appinsightsKey));
                var allRoutes = [{
                    url: "",
                    componentName: "dashboard"
                }, {
                    url: "about",
                    componentName: "about"
                }, {
                    url: "product/{productName}",
                    componentName: "product"
                }, {
                    url: "solutions/types",
                    componentName: "selectSolution"
                }, {
                    url: "solutions/types/{solutionTypeId}",
                    componentName: "createSolution"
                }, {
                    url: "solutions/delete/{solutionName}",
                    componentName: "deleteSolution"
                }],
                    app = IoT.injector.resolve("app");
                allRoutes.forEach(function (routeConfig) {
                    router.addRouteHandler(routeConfig.url, function (requestParams) {
                        app.navigateTo(routeConfig.componentName, requestParams)
                    })
                }), jQuery(function () {
                    router.startListening(), ko.applyBindings(app), app.initialize()
                })
            }
            IoT.injector = new IoT.InversifyInjector, IoT.bootstrap = bootstrap
        }(IoT = Azure.IoT || (Azure.IoT = {}))
    }(Azure = Microsoft.Azure || (Microsoft.Azure = {}))
}(Microsoft || (Microsoft = {})), ko.bindingHandlers.content = {
    init: function (element, valueAccessor) {
        ko.utils.setHtml(element, valueAccessor())
    }
}, ko.bindingHandlers.indicator = {
    init: function (element, valueAccessor) {
        var className, value = valueAccessor();
        switch (value) {
            case "Created":
            case "OrchestrationStarting":
            case "ProvisionAAD":
            case "ProvisionML":
            case "ProvisionAzure":
            case "Starting":
            case "Stopping":
            case "Deleting":
            case "Waiting":
            case "Running":
                className = "indicator-progress";
                break;
            case "Complete":
            case "Succeeded":
                className = "indicator-success";
                break;
            case "Invalid":
                className = "indicator-warning";
                break;
            case "Failed":
                className = "indicator-danger";
                break;
            default:
                throw "Unknown solution state {0}.".format(value)
        }
        $(element).addClass(className)
    }
}, ko.bindingHandlers.popover = {
    init: function (element, valueAccessor) {
        var options = valueAccessor(),
            defaults = {
                placement: "auto",
                trigger: "focus",
                viewport: {
                    selector: ".drawer-body",
                    padding: 10
                }
            };
        options = $.extend(defaults, options), $(element).popover(options)
    }
};
var Microsoft;
! function (Microsoft) {
    var Azure;
    ! function (Azure) {
        var IoT;
        ! function (IoT) {
            ko.bindingHandlers.traceClick = {
                init: function (element, valueAccessor) {
                    var eventName = valueAccessor(),
                        logger = IoT.injector.resolve("logger");
                    ko.applyBindingsToNode(element, {
                        click: function () {
                            return logger.trackEvent(eventName), !0
                        }
                    })
                }
            }
        }(IoT = Azure.IoT || (Azure.IoT = {}))
    }(Azure = Microsoft.Azure || (Microsoft.Azure = {}))
}(Microsoft || (Microsoft = {}));
var Microsoft;
! function (Microsoft) {
    var Azure;
    ! function (Azure) {
        var IoT;
        ! function (IoT) {
            var CorrelationIdKey = "correlation-id",
                Utils = function () {
                    function Utils() { }
                    return Object.defineProperty(Utils, "correlationId", {
                        get: function () {
                            return Utils.cachedCorrelationId || (Utils.cachedCorrelationId = Utils.guid(), sessionStorage.setItem(CorrelationIdKey, Utils.cachedCorrelationId)), Utils.cachedCorrelationId
                        },
                        enumerable: !0,
                        configurable: !0
                    }), Utils.guid = function () {
                        function segment() {
                            return Math.floor(65536 * (1 + Math.random())).toString(16).substring(1)
                        }
                        return "{0}{1}-{2}-{3}-{4}-{5}{6}{7}".format(segment(), segment(), segment(), segment(), segment(), segment(), segment(), segment())
                    }, Utils.isMooncake = function () {
                        var urlString = window.location.hostname,
                            cnDomain = ".cn";
                        return urlString.slice(-cnDomain.length) === cnDomain
                    }, Utils.cachedCorrelationId = sessionStorage.getItem(CorrelationIdKey), Utils
                }();
            IoT.Utils = Utils
        }(IoT = Azure.IoT || (Azure.IoT = {}))
    }(Azure = Microsoft.Azure || (Microsoft.Azure = {}))
}(Microsoft || (Microsoft = {}));
var Microsoft;
! function (Microsoft) {
    var Azure;
    ! function (Azure) {
        var IoT;
        ! function (IoT) {
            var Message = function () {
                function Message() { }
                return Message
            }();
            IoT.Message = Message
        }(IoT = Azure.IoT || (Azure.IoT = {}))
    }(Azure = Microsoft.Azure || (Microsoft.Azure = {}))
}(Microsoft || (Microsoft = {}));
var Microsoft;
! function (Microsoft) {
    var Azure;
    ! function (Azure) {
        var IoT;
        ! function (IoT) {
            var Solution = function () {
                function Solution() { }
                return Solution.prototype.indicator = function () {
                    switch (this.state) {
                        case "Provisioning":
                        case "Created":
                        case "OrchestrationStarting":
                        case "ProvisionAAD":
                        case "ProvisionML":
                        case "ProvisionAzure":
                        case "Starting":
                        case "Stopping":
                        case "Deleting":
                            return "indicator-progress";
                        case "Ready":
                        case "Complete":
                            return "indicator-success";
                        case "Invalid":
                            return "indicator-warning";
                        case "Failed":
                            return "indicator-danger";
                        default:
                            throw "Unknown solution state {0}.".format(this.state)
                    }
                }, Solution.prototype.template = function () {
                    switch (this.state) {
                        case "Created":
                        case "OrchestrationStarting":
                        case "ProvisionAAD":
                        case "ProvisionML":
                        case "ProvisionAzure":
                            return "Provisioning";
                        case "Complete":
                        case "Starting":
                        case "Stopping":
                        case "Invalid":
                            return "Ready";
                        case "Failed":
                            return "Failed";
                        case "Deleting":
                            return "Deleting";
                        default:
                            throw "Unknown solution state {0}.".format(this.state)
                    }
                }, Solution.prototype.stateName = function () {
                    switch (this.state) {
                        case "Created":
                        case "OrchestrationStarting":
                        case "ProvisionAAD":
                        case "ProvisionML":
                        case "ProvisionAzure":
                            return Resources.IndicatorLabelProvisioning;
                        case "Complete":
                            return Resources.IndicatorLabelReady;
                        case "Starting":
                            return Resources.IndicatorLabelStarting;
                        case "Stopping":
                            return Resources.IndicatorLabelStopping;
                        case "Invalid":
                            return Resources.IndicatorLabelInvalid;
                        case "Failed":
                            return Resources.IndicatorLabelFailed;
                        case "Deleting":
                            return Resources.IndicatorLabelDeleting;
                        default:
                            throw "Unknown solution state {0}.".format(this.state)
                    }
                }, Solution
            }();
            IoT.Solution = Solution
        }(IoT = Azure.IoT || (Azure.IoT = {}))
    }(Azure = Microsoft.Azure || (Microsoft.Azure = {}))
}(Microsoft || (Microsoft = {}));
var Microsoft;
! function (Microsoft) {
    var Azure;
    ! function (Azure) {
        var IoT;
        ! function (IoT) {
            IoT.SolutionTypes = {
                RemoteMonitoring: "RM",
                RemoteMonitoringStatic: "RS",
                PredictiveMaintenance: "PM",
                RemoteMonitoringV2: "RM2"
            }
        }(IoT = Azure.IoT || (Azure.IoT = {}))
    }(Azure = Microsoft.Azure || (Microsoft.Azure = {}))
}(Microsoft || (Microsoft = {}));


[{
    "id": "RM",
    "name": "Remote monitoring",
    "description": "Connect and monitor your devices to analyze untapped data and improve business outcomes by automating processes.",
    "shortDescription": "Monitor events and conditions from your devices in the field.",
    "illustrationUrl": "/content/styles/img/RemoteMonitoring.jpg",
    "githubUrl": "https://github.com/Azure/azure-iot-remote-monitoring",
    "price": 1500.0,
    "properties": [{
        "name": "Number of Devices",
        "value": "1,000"
    }, {
        "name": "Number of Messages/Day",
        "value": "3 Million"
    }, {
        "name": "Message Frequency",
        "value": "Every 30 sec"
    }],
    "services": ["1 Azure Active Directory application", "1 IoT Hub (S2 - Standard tier)", "1 DocumentDB Account (S1)", "2 Event Hubs (Basic throughput unit)", "1 Storage account (Standard-GRS)", "3 Stream Analytics jobs (1 streaming unit per job)", "1 Azure App Service Web App for Website (P1 - Premium: 2 small)", "1 Azure App Service Web App for Web jobs (S1 - Standard: 2 small) running 25 simulated devices by default"],
    "standardServices": null,
    "isTeaser": false,
    "terms": "<p>In addition to the above Azure services, creating a solution will result in your being signed up for a subscription to the following Azure Marketplace offering(s), which are subject to the following terms:</p><p><a href=\"https://azure.microsoft.com/en-us/marketplace/partners/bingmaps/mapapis\">Bing Maps API for Enterprise (Internal Website Transactions Level 1)</a>: <a href=\"http://www.microsoft.com/maps/product/terms.html\">terms of use</a> and <a href=\"http://www.microsoft.com/en-us/privacystatement/default.aspx\">privacy statement</a>.</p>",
    "additionalOptions": null,
    "resources": ["microsoft.devices", "microsoft.storage", "microsoft.streamanalytics", "microsoft.eventhub", "microsoft.documentdb", "microsoft.web"],
    "bingMapRequired": true
}, {
    "id": "RM2",
    "name": "Remote monitoring",
    "description": "Connect and monitor your devices to analyze untapped data and improve business outcomes by automating processes.",
    "shortDescription": "Monitor events and conditions from your devices in the field.",
    "illustrationUrl": "/content/styles/img/RemoteMonitoringV2.png",
    "githubUrl": "https://github.com/Azure/azure-iot-remote-monitoring",
    "price": 1500.0,
    "properties": [{
        "name": "Number of Devices",
        "value": "1,000"
    }, {
        "name": "Number of Messages/Day",
        "value": "3 Million"
    }, {
        "name": "Message Frequency",
        "value": "Every 30 sec"
    }],
    "services": ["1 Azure Active Directory application", "1 Virtual Machine (Standard D1 V2 (1 core, 3.5 GB memory))", "1 IoT Hub (S1 - Basic tier)", "1 Cosmos DB Account (Standard)", "1 Storage account (Standard-GRS)", "1 Web Application"],
    "standardServices": ["1 Azure Active Directory application", "1 Azure Container Service ", "3 Virtual Machines (Standard D1 V2 (1 core, 3.5 GB memory))", "1 IoT Hub (S1 - Basic tier)", "1 Cosmos DB Account (Standard)", "1 Storage account (Standard-GRS)", "1 Web Application"],
    "isTeaser": false,
    "terms": "<p>In addition to the above Azure services, creating a solution will result in your being signed up for a subscription to the following Azure Marketplace offering(s), which are subject to the following terms:</p><p><a href=\"https://azure.microsoft.com/en-us/marketplace/partners/bingmaps/mapapis\">Bing Maps API for Enterprise (Internal Website Transactions Level 1)</a>: <a href=\"http://www.microsoft.com/maps/product/terms.html\">terms of use</a> and <a href=\"http://www.microsoft.com/en-us/privacystatement/default.aspx\">privacy statement</a>.</p>",
    "additionalOptions": null,
    "resources": ["microsoft.devices", "microsoft.storage", "microsoft.documentdb", "microsoft.compute/virtualMachines/Standard_D1_v2", "microsoft.web"],
    "bingMapRequired": true
}, {
    "id": "CF",
    "name": "Connected factory",
    "description": "Accelerate your journey to Industrie 4.0 – connect, monitor and control industrial devices for insights using OPC UA to drive operational productivity and profitability.",
    "shortDescription": "Connect, monitor and control industrial devices using OPC UA.",
    "illustrationUrl": "/content/styles/img/ConnectedFactory.jpg",
    "githubUrl": "https://github.com/Azure/azure-iot-connected-factory",
    "price": 0.0,
    "properties": [],
    "services": ["1 Storage account (Standard-LRS)", "1 Virtual Machine (Standard D1 v2 (1 core, 3.5 GB memory))", "1 IoT Hub (Standard S1, 3 units)", "1 Key vault (Standard)", "1 Azure Time Series Insights (Standard S1)", "1 Web App Service (Standard S1)"],
    "standardServices": null,
    "isTeaser": false,
    "terms": "<p>In addition to the above Azure services, creating a solution will result in your being signed up for a subscription to the following Azure Marketplace offering(s), which are subject to the following terms:</p><p><a href=\"https://azure.microsoft.com/en-us/marketplace/partners/bingmaps/mapapis\">Bing Maps API for Enterprise (Internal Website Transactions Level 1)</a>: <a href=\"http://www.microsoft.com/maps/product/terms.html\">terms of use</a> and <a href=\"http://www.microsoft.com/en-us/privacystatement/default.aspx\">privacy statement</a>.</p>",
    "additionalOptions": null,
    "resources": ["microsoft.devices", "microsoft.storage", "microsoft.web", "microsoft.timeseriesinsights/environments", "microsoft.compute/virtualMachines/Standard_D1_v2"],
    "bingMapRequired": true
}, {
    "id": "PM",
    "name": "Predictive maintenance",
    "description": "Anticipate maintenance needs and avoid unscheduled downtime by connecting and monitoring your devices for predictive maintenance.",
    "shortDescription": "Identify potential equipment failure and optimize maintenance",
    "illustrationUrl": "/content/styles/img/PredictiveMaintenance.jpg",
    "githubUrl": "https://github.com/Azure/azure-iot-predictive-maintenance",
    "price": 2500.0,
    "properties": [{
        "name": "Number of Devices",
        "value": "1,000"
    }, {
        "name": "Number of Messages/Day",
        "value": "3 Million"
    }, {
        "name": "Message Frequency",
        "value": "Every 30 sec"
    }, {
        "name": "Machine Learning Predictions/Day",
        "value": "24,000"
    }],
    "services": ["1 Azure Active Directory application", "1 IoT Hub (S2 - Standard tier)", "1 Event Hub (Basic throughput unit)", "2 Storage accounts (Standard-GRS)", "1 Stream Analytics job (1 streaming unit)", "1 App service plans (S1 - Standard: 2 small)", "1 App service plan (P1 - Premium: 2 small)", "1 Web app (included in App Service plan)", "1 Azure App Service Web App for Website (P1 - Premium: 2 small)", "1 Azure App Service Web App for Web jobs (S1 - Standard: 2 small)"],
    "standardServices": null,
    "isTeaser": false,
    "terms": "",
    "additionalOptions": null,
    "resources": ["microsoft.devices", "microsoft.storage", "microsoft.streamanalytics", "microsoft.eventhub", "microsoft.documentdb", "microsoft.web"],
    "bingMapRequired": false
}]