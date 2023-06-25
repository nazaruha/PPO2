import { useEffect, useState } from "react";
import http from "../../../../http_common";
import { ICustomerItem, ICustomerSearch, ICustomerSearchResult, ICustomerServiceResponse } from "../types";
import { Link, useSearchParams } from "react-router-dom";
import classNames from "classnames";
import ModalDelete from "../../../common/ModalDelete";
import { useFormik } from "formik";

const CustomerListPage = () => {
    const [searchParams, setSearchParams] = useSearchParams();

    const projectId = localStorage.chosenProjectId;
    const [search, setSearch] = useState<ICustomerSearch>({
        firstName: searchParams.get("firstName") || "",
        secondName: searchParams.get("secondName") || "",
        address: searchParams.get("address") || "",
        email: searchParams.get("email") || "", //3
        phone: searchParams.get("phone") || "", //3
        page: searchParams.get("page") || 1,
        projectId: searchParams.get("projectId") || Number(projectId),
        id: searchParams.get("id") || 0
    });

    const onSubmitSearch = (values: ICustomerSearch) => {
        console.log("Проводимо пошук", values);
        setSearch(values);
    }

    const formik = useFormik({
        initialValues: search,
        onSubmit: onSubmitSearch
    });

    const { values, touched, errors, handleSubmit, handleChange } = formik;

    const [searchResult, setSearchResult] = useState<ICustomerSearchResult>({
        customers: [],
        pages: 0,
        currentPage: 0,
        total: 0,
        projectId: projectId,
        projectName: "",
    });

    useEffect(() => {
        console.log(search);
        http.get<ICustomerSearchResult>(`api/customer/search`, {
            params: search
        })
            .then(resp => {
                const { data } = resp;
                console.log("Server response", data);
                setSearchResult(data);
            }).catch(err => {
                console.log("ERR", err);
            })
    }, [search]);

    const { customers, pages, currentPage } = searchResult;

    const buttons = [];
    for (let i = 1; i <= pages; i++) {
        buttons.push(i);
    }

    const pagination = buttons.map(x => {
        return (
            <li className={classNames(
                "page-item",
                { "active": x === currentPage }
            )} key={x}>
                <Link className="page-link" to={`?page=${x}`} onClick={() => setSearch({ ...search, page: x })}>
                    {x}
                </Link>
            </li>
        );
    })

    const onDeleteCustomer = async (customerId: number) => {
        console.log("DELETE CUSTOMER ID", customerId);
        try {
            await http.delete(`api/customerproject/delete/customer-${customerId}/project-${projectId}`);
            setSearch({ ...search, page: 1 });
        } catch (err) {
            console.log("ERR DELETE", err);
        }
    }

    const tableViewList = customers.map((item) => {
        return (
            <tr key={item.id}>
                <th scope="row">{item.id}</th>
                <td>{item.firstName}</td>
                <td>{item.secondName}</td>
                <td>{item.phone}</td>
                <td>{item.email}</td>
                {item.address ? (
                    <td>{item.address}</td>
                ) : (
                    <td>---------</td>
                )}
                <td style={{ display: 'flex' }}>
                    <Link to={`edit/${item.id}`} className="btn btn-warning"
                    >
                        Змінити <i className="fa fa-pencil" aria-hidden="true"></i>
                    </Link>
                    &nbsp;
                    &nbsp;
                    &nbsp;
                    <ModalDelete
                        id={item.id}
                        text={`Ви дійсно бажаєте видалити користувача '${item.firstName} ${item.secondName}'?`}
                        deleteFunc={onDeleteCustomer}
                    />
                </td>
            </tr>
        )
    })

    return (
        <>
            <h1 className="text-center">Список користувачів</h1>

            <form onSubmit={handleSubmit} className="mb-5">
                <div className="row">
                    <div className="col-md-2">
                        <label htmlFor="firstName" className="form-label">Ім'я</label>
                        <input
                            type="text"
                            id="firstName"
                            name="firstName"
                            value={values.firstName}
                            placeholder="Пошук по назві"
                            className={classNames(
                                "form-control",
                                { "is-invalid": errors.firstName && touched.firstName }
                            )}
                            onChange={handleChange}
                        />
                    </div>
                    <div className="col-md-2">
                        <label htmlFor="secondName" className="form-label">Фамілія</label>
                        <input
                            type="text"
                            id="secondName"
                            name="secondName"
                            value={values.secondName}
                            placeholder="Пошук по фамілії"
                            className={classNames(
                                "form-control",
                                { "is-invalid": errors.secondName && touched.secondName }
                            )}
                            onChange={handleChange}
                        />
                    </div>
                    <div className="col-md-2">
                        <label htmlFor="email" className="form-label">Пошта</label>
                        <input
                            type="text"
                            id="email"
                            name="email"
                            value={values.email}
                            placeholder="Пошук по пошті"
                            className={classNames(
                                "form-control",
                                { "is-invalid": errors.email && touched.email }
                            )}
                            onChange={handleChange}
                        />
                    </div>
                    <div className="col-md-2">
                        <label htmlFor="phone" className="form-label">Телефон</label>
                        <input
                            type="text"
                            id="phone"
                            name="phone"
                            value={values.phone}
                            placeholder="Пошук по телефону"
                            className={classNames(
                                "form-control",
                                { "is-invalid": errors.phone && touched.phone }
                            )}
                            onChange={handleChange}
                        />
                    </div>
                    <div className="col-md-2">
                        <label htmlFor="address" className="form-label">Адреса</label>
                        <input
                            type="text"
                            id="address"
                            name="address"
                            value={values.address}
                            placeholder="Пошук по адресі"
                            className={classNames(
                                "form-control",
                                { "is-invalid": errors.address && touched.address }
                            )}
                            onChange={handleChange}
                        />
                    </div>
                    <div className="col-md-2" style={{ display: 'flex', alignItems: 'end' }}>
                        <button type="submit" className="btn btn-primary">Пошук</button>
                    </div>
                </div>
            </form>

            <Link to="create" className="btn btn-success fs-5">Додати <i className="fa fa-plus-square-o fs-6" aria-hidden="true"></i></Link>
            <table className="table">
                <thead>
                    <tr>
                        <th scope="col">#</th>
                        <th scope="col">Ім'я</th>
                        <th scope="col">Фамілія</th>
                        <th scope="col">Телефон</th>
                        <th scope="col">Пошта</th>
                        <th scope="col">Адреса</th>
                        <th style={{ width: '20%' }}></th>
                    </tr>
                </thead>
                <tbody>
                    {tableViewList}
                </tbody>
            </table>

            <nav aria-label="..." style={{ display: 'flex', justifyContent: 'end' }}>
                <ul className="pagination">
                    <li className={classNames(
                        "page-item",
                        { "disabled": search.page == 1 }
                    )}>
                        <span className="page-link">
                            <Link to={`?page=${Number(search.page) - 1}`} onClick={() => setSearch({ ...search, page: Number(search.page) - 1 })} className="fa fa-arrow-left text-primary" style={{ textDecoration: "none" }} aria-hidden="true"></Link>
                        </span>
                    </li>
                    {pagination}
                    <li className={classNames(
                        "page-item",
                        { "disabled": search.page == buttons.length }
                    )}>
                        <span className="page-link">
                            <Link to={`?page=${Number(search.page) + 1}`} onClick={() => setSearch({ ...search, page: Number(search.page) + 1 })} className="fa fa-arrow-right text-primary" style={{ textDecoration: "none" }} aria-hidden="true"></Link>
                        </span>
                    </li>
                </ul>
            </nav>
        </>
    )
}

export default CustomerListPage;