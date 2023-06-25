import { useSelector } from "react-redux";
import { Link, NavLink, useNavigate } from "react-router-dom";
import { IStoreProject, StoreProjectActionType } from "../../pages/Project/types";
import { store } from "../../containers/store";

const DefaultSideBar = () => {
    const navigate = useNavigate();
    const { isProjectStored, project } = useSelector((store: any) => store.project as IStoreProject);

    const onQuitProjectClick = () => {
        localStorage.removeItem("chosenProjectId");
        localStorage.removeItem("chosenProjectName");
        store.dispatch({ type: StoreProjectActionType.STORE_REMOVE_PROJECT, isProjectStored: false, payload: undefined });
        console.log("Remove project from store DefaultSideBar.tsx");
        navigate("../");
        // window.location.reload();
    }

    return (
        <>
            <div className="col-auto col-md-3 col-xl-2 px-sm-2 px-0 bg-dark">
                <div className="d-flex flex-column align-items-center align-items-sm-start px-3 pt-2 text-white min-vh-100">
                    <a href="/" className="d-flex align-items-center pb-3 mb-md-0 me-md-auto text-white text-decoration-none">
                        <span className="fs-5 d-none d-sm-inline">Menu</span>
                    </a>
                    <ul className="nav nav-pills flex-column mb-sm-auto mb-0 align-items-center align-items-sm-start" id="menu">
                        <li className="nav-item">
                            <Link to="/main-page" className="nav-link align-middle px-0">
                                <i className="fa fa-home" aria-hidden="true"></i>
                                <span className="ms-1 d-none d-sm-inline">Головна</span>
                            </Link>
                        </li>
                        <li>
                            <Link
                                to="#submenu1"
                                data-bs-toggle="collapse"
                                className="nav-link px-0 align-middle"
                            >
                                <i className="fa fa-bars" aria-hidden="true"></i>
                                <span className="ms-1 d-none d-sm-inline">Користувачі</span>
                            </Link>
                            <ul className="collapse show nav flex-column ms-1" id="submenu1" data-bs-parent="#menu">
                                <li className="w-100">
                                    <Link
                                        to="customer"
                                        className="nav-link px-0"
                                    >
                                        <i className="fa fa-list-alt ms-4 me-2" aria-hidden="true"></i>
                                        <span className="d-none d-sm-inline">Список</span>
                                    </Link>
                                </li>
                                <li>
                                    <Link
                                        to="customer/create"
                                        className="nav-link px-0"
                                    >
                                        <i className="fa fa-plus-square ms-4 me-2" aria-hidden="true"></i>
                                        <span className="d-none d-sm-inline">Додати</span>
                                    </Link>
                                </li>
                            </ul>
                        </li>
                        <li>
                            <Link
                                to="#submenu2"
                                data-bs-toggle="collapse"
                                className="nav-link px-0 align-middle"
                            >
                                <i className="fa fa-bars" aria-hidden="true"></i>
                                <span className="ms-1 d-none d-sm-inline">Виробники</span>
                            </Link>
                            <ul className="collapse show nav flex-column ms-1" id="submenu2" data-bs-parent="#menu">
                                <li className="w-100">
                                    <Link
                                        to="manufacturer"
                                        className="nav-link px-0"
                                    >
                                        <i className="fa fa-list-alt ms-4 me-2" aria-hidden="true"></i>
                                        <span className="d-none d-sm-inline">Список</span>
                                    </Link>
                                </li>
                                <li>
                                    <Link
                                        to="manufacturer/create"
                                        className="nav-link px-0"
                                    >
                                        <i className="fa fa-plus-square ms-4 me-2" aria-hidden="true"></i>
                                        <span className="d-none d-sm-inline">Додати</span>
                                    </Link>
                                </li>
                            </ul>
                        </li>
                        <li>
                            <Link
                                to="#submenu2"
                                data-bs-toggle="collapse"
                                className="nav-link px-0 align-middle"
                            >
                                <i className="fa fa-bars" aria-hidden="true"></i>
                                <span className="ms-1 d-none d-sm-inline">Продукти</span>
                            </Link>
                            <ul className="collapse show nav flex-column ms-1" id="submenu2" data-bs-parent="#menu">
                                <li className="w-100">
                                    <Link
                                        to="product"
                                        className="nav-link px-0"
                                    >
                                        <i className="fa fa-list-alt ms-4 me-2" aria-hidden="true"></i>
                                        <span className="d-none d-sm-inline">Список</span>
                                    </Link>
                                </li>
                                <li>
                                    <Link
                                        to="product/create"
                                        className="nav-link px-0"
                                    >
                                        <i className="fa fa-plus-square ms-4 me-2" aria-hidden="true"></i>
                                        <span className="d-none d-sm-inline">Додати</span>
                                    </Link>
                                </li>
                            </ul>
                        </li>
                        <li>
                            <Link
                                to="#submenu2"
                                data-bs-toggle="collapse"
                                className="nav-link px-0 align-middle"
                            >
                                <i className="fa fa-bars" aria-hidden="true"></i>
                                <span className="ms-1 d-none d-sm-inline">Сховище</span>
                            </Link>
                            <ul className="collapse show nav flex-column ms-1" id="submenu2" data-bs-parent="#menu">
                                <li className="w-100">
                                    <Link
                                        to="storage"
                                        className="nav-link px-0"
                                    >
                                        <i className="fa fa-list-alt ms-4 me-2" aria-hidden="true"></i>
                                        <span className="d-none d-sm-inline">Список</span>
                                    </Link>
                                </li>
                                <li>
                                    <Link
                                        to="storage/create"
                                        className="nav-link px-0"
                                    >
                                        <i className="fa fa-plus-square ms-4 me-2" aria-hidden="true"></i>
                                        <span className="d-none d-sm-inline">Додати</span>
                                    </Link>
                                </li>
                            </ul>
                        </li>
                        <li>
                            <Link
                                to="#submenu2"
                                data-bs-toggle="collapse"
                                className="nav-link px-0 align-middle"
                            >
                                <i className="fa fa-bars" aria-hidden="true"></i>
                                <span className="ms-1 d-none d-sm-inline">Замовлення</span>
                            </Link>
                            <ul className="collapse show nav flex-column ms-1" id="submenu2" data-bs-parent="#menu">
                                <li className="w-100">
                                    <Link
                                        to="order"
                                        className="nav-link px-0"
                                    >
                                        <i className="fa fa-list-alt ms-4 me-2" aria-hidden="true"></i>
                                        <span className="d-none d-sm-inline">Список</span>
                                    </Link>
                                </li>
                                <li>
                                    <Link
                                        to="order/create"
                                        className="nav-link px-0"
                                    >
                                        <i className="fa fa-plus-square ms-4 me-2" aria-hidden="true"></i>
                                        <span className="d-none d-sm-inline">Додати</span>
                                    </Link>
                                </li>
                            </ul>
                        </li>
                    </ul>
                    <hr />
                    {/* <div className="dropdown pb-4">
                        <a href="#" className="d-flex align-items-center text-white text-decoration-none dropdown-toggle" id="dropdownUser1" data-bs-toggle="dropdown" aria-expanded="false">
                            <img src="https://github.com/mdo.png" alt="hugenerd" width="30" height="30" className="rounded-circle" />
                            <span className="d-none d-sm-inline mx-1">{project?.name}</span>
                        </a>
                        <ul className="dropdown-menu dropdown-menu-dark text-small shadow" aria-labelledby="dropdownUser1">
                            <Link className="dropdown-item" to="project/profile">Профіль</Link>
                            <li>
                                <hr className="dropdown-divider" />
                            </li>
                            <li onClick={onQuitProjectClick} className="dropdown-item">Вийти</li>
                        </ul>
                    </div> */}
                    <div className="mb-3">
                        {project?.name}
                        <i className="fa fa-power-off ms-3" aria-hidden="true" onClick={onQuitProjectClick}></i>
                    </div>

                </div>
            </div>
        </>
    )
}

export default DefaultSideBar;