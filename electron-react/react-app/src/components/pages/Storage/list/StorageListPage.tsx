import { useEffect, useState } from "react";
import { Link, useSearchParams } from "react-router-dom";
import { IStorageSearch, IStorageSearchResult } from "../types";
import { useFormik } from "formik";
import http from "../../../../http_common";
import classNames from "classnames";
import ModalDelete2 from "../../../common/ModalDelete2";

const StorageListPage = () => {
    const todayDate = new Date();
    const [searchParams, setSearchParams] = useSearchParams();
    const projectId = localStorage.chosenProjectId;

    const [search, setSearch] = useState<IStorageSearch>({
        productName: searchParams.get("productName") || "",
        manufacturerName: searchParams.get("projectName") || "",
        description: searchParams.get("description") || "",
        count: searchParams.get("count") || 0,
        price: searchParams.get("price") || 0,
        page: searchParams.get("page") || 1,
    });

    const onSubmitSearch = (values: IStorageSearch) => {
        console.log("Проводимо пошук", values);
        setSearch(values);
    }

    const formik = useFormik({
        initialValues: search,
        onSubmit: onSubmitSearch
    });

    const { values, touched, errors, handleSubmit, handleChange } = formik;

    const [searchResult, setSearchResult] = useState<IStorageSearchResult>({
        storage: [],
        pages: 0,
        currentPage: 0,
        total: 0,
    });

    useEffect(() => {
        // console.log("TIME!!!!", todayDate);
        console.log(search);
        http.get<IStorageSearchResult>(`api/storage/search/${projectId}`, {
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

    const { storage, pages, currentPage } = searchResult;

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

    const onDeleteStorage = async (productId: number, projectId: number) => {
        try {
            await http.delete(`api/storage/delete/product-${productId}/project-${projectId}`);
            setSearch({ ...search, page: 1 });
        } catch (err) {
            console.log("ERR DELETE", err);
        }
    }

    const tableViewList = storage.map((item, index) => {
        return (
            <tr key={index} className="text-center">
                <th scope="row">{item.productId}</th>
                <td>{item.product.name}</td>
                <td>{item.product.manufacturerName}</td>
                {item.description ? (
                    <td>{item.description}</td>
                ) : (
                    <td>------------</td>
                )}
                <td>{item.price}</td>
                {item.count <= 0 ? (
                    <td className="text-danger"><b>{item.count}</b></td>
                ) : (
                    <td>{item.count}</td>
                )}

                {
                    todayDate > new Date(item.expireDate) ? (
                        <td className="text-danger"><b>{String(item.expireDate)}</b></td>
                    ) : (
                        <td>{String(item.expireDate)}</td>
                    )
                }

                <td style={{ display: 'flex', justifyContent: 'center' }}>
                    <Link to={`edit/${item.productId}/${item.product.manufacturerId}/${item.projectId}`} className="btn btn-warning"
                    >
                        Змінити <i className="fa fa-pencil" aria-hidden="true"></i>
                    </Link>
                    &nbsp;
                    &nbsp;
                    &nbsp;
                    <ModalDelete2
                        childId={item.productId}
                        parentId={item.projectId}
                        text={`Ви дійсно бажаєте видалити Продукт '${item.product.name}' з даного виробника '${item.product.manufacturerName}'?`}
                        deleteFunc={onDeleteStorage}
                    />
                </td>
            </tr>
        )
    })

    return (
        <>
            <h1 className="text-center">Сховище</h1>

            <form onSubmit={handleSubmit} className="mb-5">
                <div className="row">
                    <div className="col-md-2">
                        <label htmlFor="productName" className="form-label">Продукт</label>
                        <input
                            type="text"
                            id="productName"
                            name="productName"
                            value={values.productName}
                            placeholder="Пошук по продукту"
                            className={classNames(
                                "form-control",
                                { "is-invalid": errors.productName && touched.productName }
                            )}
                            onChange={handleChange}
                        />
                    </div>
                    <div className="col-md-2">
                        <label htmlFor="manufacturerName" className="form-label">Виробник</label>
                        <input
                            type="text"
                            id="manufacturerName"
                            name="manufacturerName"
                            value={values.manufacturerName}
                            placeholder="Пошук по виробнику"
                            className={classNames(
                                "form-control",
                                { "is-invalid": errors.manufacturerName && touched.manufacturerName }
                            )}
                            onChange={handleChange}
                        />
                    </div>
                    <div className="col-md-2">
                        <label htmlFor="description" className="form-label">Опис</label>
                        <input
                            type="text"
                            id="description"
                            name="description"
                            value={values.description}
                            placeholder="Пошук по опису"
                            className={classNames(
                                "form-control",
                                { "is-invalid": errors.description && touched.description }
                            )}
                            onChange={handleChange}
                        />
                    </div>
                    <div className="col-md-2">
                        <label htmlFor="count" className="form-label">Кількість</label>
                        <input
                            type="number"
                            id="count"
                            name="count"
                            value={values.count}
                            placeholder="Пошук по кількості"
                            className={classNames(
                                "form-control",
                                { "is-invalid": errors.count && touched.count }
                            )}
                            onChange={handleChange}
                        />
                    </div>
                    <div className="col-md-2">
                        <label htmlFor="price" className="form-label">Ціна</label>
                        <input
                            type="number"
                            id="price"
                            name="price"
                            value={values.price}
                            placeholder="Пошук по ціні"
                            className={classNames(
                                "form-control",
                                { "is-invalid": errors.price && touched.price }
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
            <table className="table mt-3">
                <thead>
                    <tr className="text-center">
                        <th scope="col" ># Продутку</th>
                        <th scope="col" >Продукт</th>
                        <th scope="col" >Виробник</th>
                        <th scope="col" >Опис</th>
                        <th scope="col" >Ціна (₴ грн)</th>
                        <th scope="col" >Кількість</th>
                        <th scope="col" >Термін призначеності</th>
                        <th></th>
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

export default StorageListPage;